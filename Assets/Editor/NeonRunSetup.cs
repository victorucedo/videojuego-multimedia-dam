using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditor.U2D.Sprites;
using UnityEngine;

/// <summary>
/// Herramienta de editor que prepara el proyecto automáticamente:
/// 1. Ordena la carpeta de Warped City y quita archivos que no se usan.
/// 2. Crea las animaciones del personaje (Idle, Run, Jump, Hurt) y su Animator.
/// 3. Crea una escena de prueba (Nivel1) con fondo parallax, suelo, plataformas y el jugador.
/// Se ejecuta desde el menú: NEON RUN > Configurar personaje y nivel de prueba
/// </summary>
public static class NeonRunSetup
{
    const string OldRoot = "Assets/Art/warped city files";
    const string OldAssets = "Assets/Art/warped city files/warped city files/Assets";
    const string WC = "Assets/Art/WarpedCity";
    const string AnimDir = "Assets/Animations/Player";

    [MenuItem("NEON RUN/Configurar personaje y nivel de prueba")]
    public static void Configurar()
    {
        try
        {
            LimpiarCarpetas();
            AssetDatabase.Refresh();

            Sprite suelo = PrepararTileset();
            Dictionary<string, Sprite> fondos = PrepararFondos();
            AnimatorController controller = CrearAnimaciones(out Sprite primerFrame);
            CrearEscena(suelo, fondos, controller, primerFrame);

            AssetDatabase.SaveAssets();
            Debug.Log("[NEON RUN] Configuración completada. Pulsa Play para probar: A/D o flechas para moverte, Espacio para saltar.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("[NEON RUN] Error en la configuración: " + e);
        }
    }

    // ---------------------------------------------------------------- Carpetas
    static void LimpiarCarpetas()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Art")) AssetDatabase.CreateFolder("Assets", "Art");

        // Sacar el contenido útil a Assets/Art/WarpedCity
        if (AssetDatabase.IsValidFolder(OldAssets) && !AssetDatabase.IsValidFolder(WC))
        {
            string err = AssetDatabase.MoveAsset(OldAssets, WC);
            if (!string.IsNullOrEmpty(err)) throw new System.Exception("No se pudo mover Warped City: " + err);
        }

        if (AssetDatabase.IsValidFolder(OldRoot))
        {
            // Conservar el PDF de la licencia (lo pide la entrega: assets referenciados)
            string[] guids = AssetDatabase.FindAssets("license", new[] { OldRoot });
            foreach (string guid in guids)
            {
                string p = AssetDatabase.GUIDToAssetPath(guid);
                if (p.Contains("__MACOSX")) continue;
                string destino = WC + "/" + Path.GetFileName(p);
                if (AssetDatabase.LoadMainAssetAtPath(destino) == null) AssetDatabase.MoveAsset(p, destino);
            }
            // Borra el resto (incluye __MACOSX). Va a la papelera de reciclaje.
            AssetDatabase.MoveAssetToTrash(OldRoot);
        }

        foreach (string sub in new[] { "PSD", "GIF PREVIEWS" })
        {
            string p = WC + "/" + sub;
            if (AssetDatabase.IsValidFolder(p)) AssetDatabase.MoveAssetToTrash(p);
        }

        CrearCarpeta("Assets/Animations");
        CrearCarpeta(AnimDir);
        CrearCarpeta("Assets/Prefabs");
        CrearCarpeta("Assets/Scenes");
    }

    static void CrearCarpeta(string ruta)
    {
        if (AssetDatabase.IsValidFolder(ruta)) return;
        string padre = Path.GetDirectoryName(ruta).Replace("\\", "/");
        AssetDatabase.CreateFolder(padre, Path.GetFileName(ruta));
    }

    // ---------------------------------------------------------------- Sprites
    static Sprite PrepararTileset()
    {
        string path = WC + "/ENVIRONMENT/tileset.png";
        var ti = (TextureImporter)AssetImporter.GetAtPath(path);
        if (ti == null) throw new System.Exception("No encuentro " + path);

        ti.textureType = TextureImporterType.Sprite;
        ti.spriteImportMode = SpriteImportMode.Multiple;
        ti.spritePixelsPerUnit = 16;
        ti.filterMode = FilterMode.Point;
        ti.textureCompression = TextureImporterCompression.Uncompressed;

        var settings = new TextureImporterSettings();
        ti.ReadTextureSettings(settings);
        settings.spriteMeshType = SpriteMeshType.FullRect;
        ti.SetTextureSettings(settings);

        ti.SaveAndReimport();

        // Baldosa de suelo de 16x16 (esquina superior derecha del tileset)
        var factory = new SpriteDataProviderFactories();
        factory.Init();
        ISpriteEditorDataProvider dp = factory.GetSpriteEditorDataProviderFromObject(ti);
        dp.InitSpriteEditorDataProvider();
        var rect = new SpriteRect
        {
            name = "suelo",
            spriteID = GUID.Generate(),
            rect = new Rect(336, 224, 16, 16),
            alignment = SpriteAlignment.Center,
            pivot = new Vector2(0.5f, 0.5f)
        };
        dp.SetSpriteRects(new[] { rect });
        var nombres = dp.GetDataProvider<ISpriteNameFileIdDataProvider>();
        if (nombres != null)
            nombres.SetNameFileIdPairs(new[] { new SpriteNameFileIdPair(rect.name, rect.spriteID) });
        dp.Apply();
        ti.SaveAndReimport();

        Sprite s = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().FirstOrDefault(x => x.name == "suelo");
        if (s == null) throw new System.Exception("No se pudo crear el sprite de suelo");
        return s;
    }

    static Dictionary<string, Sprite> PrepararFondos()
    {
        var res = new Dictionary<string, Sprite>();
        foreach (string nombre in new[] { "skyline-a", "buildings-bg", "near-buildings-bg" })
        {
            string path = WC + "/ENVIRONMENT/background/" + nombre + ".png";
            var ti = (TextureImporter)AssetImporter.GetAtPath(path);
            if (ti == null) throw new System.Exception("No encuentro " + path);
            var settings = new TextureImporterSettings();
            ti.ReadTextureSettings(settings);
            settings.spriteMeshType = SpriteMeshType.FullRect;
            ti.SetTextureSettings(settings);
            ti.wrapMode = TextureWrapMode.Repeat;
            ti.SaveAndReimport();
            res[nombre] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
        return res;
    }

    // ------------------------------------------------------------- Animaciones
    static AnimatorController CrearAnimaciones(out Sprite primerFrame)
    {
        string pf = WC + "/SPRITES/player";
        Sprite[] idleF = Frames(pf + "/idle/idle-", 4);
        Sprite[] runF = Frames(pf + "/run/run-", 8);
        Sprite[] jumpF = Frames(pf + "/jump/jump-", 4);
        Sprite hurtS = Cargar(pf + "/hurt/hurt.png");
        primerFrame = idleF[0];

        AnimationClip idle = CrearClip("Idle", idleF, 8, true);
        AnimationClip run = CrearClip("Run", runF, 12, true);
        AnimationClip jump = CrearClip("Jump", jumpF, 10, false);
        AnimationClip hurt = CrearClip("Hurt", new[] { hurtS }, 3, false);

        string cpath = AnimDir + "/Player.controller";
        AssetDatabase.DeleteAsset(cpath);
        AnimatorController ac = AnimatorController.CreateAnimatorControllerAtPath(cpath);
        ac.AddParameter("Speed", AnimatorControllerParameterType.Float);
        ac.AddParameter(new AnimatorControllerParameter
        {
            name = "Grounded",
            type = AnimatorControllerParameterType.Bool,
            defaultBool = true
        });
        ac.AddParameter("Hurt", AnimatorControllerParameterType.Trigger);

        AnimatorStateMachine sm = ac.layers[0].stateMachine;
        AnimatorState sIdle = sm.AddState("Idle", new Vector3(300, 0, 0));
        AnimatorState sRun = sm.AddState("Run", new Vector3(300, 120, 0));
        AnimatorState sJump = sm.AddState("Jump", new Vector3(560, 60, 0));
        AnimatorState sHurt = sm.AddState("Hurt", new Vector3(60, 200, 0));
        sIdle.motion = idle; sRun.motion = run; sJump.motion = jump; sHurt.motion = hurt;
        sm.defaultState = sIdle;

        Transicion(sIdle, sRun).AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        Transicion(sRun, sIdle).AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        Transicion(sIdle, sJump).AddCondition(AnimatorConditionMode.IfNot, 0, "Grounded");
        Transicion(sRun, sJump).AddCondition(AnimatorConditionMode.IfNot, 0, "Grounded");
        Transicion(sJump, sIdle).AddCondition(AnimatorConditionMode.If, 0, "Grounded");

        AnimatorStateTransition aHurt = sm.AddAnyStateTransition(sHurt);
        aHurt.hasExitTime = false;
        aHurt.duration = 0;
        aHurt.canTransitionToSelf = false;
        aHurt.AddCondition(AnimatorConditionMode.If, 0, "Hurt");

        AnimatorStateTransition hurtFin = sHurt.AddTransition(sIdle);
        hurtFin.hasExitTime = true;
        hurtFin.exitTime = 1f;
        hurtFin.duration = 0;

        EditorUtility.SetDirty(ac);
        return ac;
    }

    static AnimatorStateTransition Transicion(AnimatorState desde, AnimatorState hasta)
    {
        AnimatorStateTransition t = desde.AddTransition(hasta);
        t.hasExitTime = false;
        t.duration = 0;
        return t;
    }

    static Sprite[] Frames(string prefijo, int n)
    {
        var lista = new Sprite[n];
        for (int i = 0; i < n; i++) lista[i] = Cargar(prefijo + (i + 1) + ".png");
        return lista;
    }

    static Sprite Cargar(string path)
    {
        Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (s == null) throw new System.Exception("No encuentro el sprite " + path);
        return s;
    }

    static AnimationClip CrearClip(string nombre, Sprite[] frames, float fps, bool bucle)
    {
        var clip = new AnimationClip { frameRate = fps };
        var binding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
        var keys = new ObjectReferenceKeyframe[frames.Length + 1];
        for (int i = 0; i < frames.Length; i++)
            keys[i] = new ObjectReferenceKeyframe { time = i / fps, value = frames[i] };
        // Repetimos el último frame para que dure lo mismo que los demás
        keys[frames.Length] = new ObjectReferenceKeyframe { time = frames.Length / fps, value = frames[frames.Length - 1] };
        AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);

        AnimationClipSettings ajustes = AnimationUtility.GetAnimationClipSettings(clip);
        ajustes.loopTime = bucle;
        AnimationUtility.SetAnimationClipSettings(clip, ajustes);

        string path = AnimDir + "/" + nombre + ".anim";
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(clip, path);
        return clip;
    }

    // ------------------------------------------------------------------ Escena
    static void CrearEscena(Sprite suelo, Dictionary<string, Sprite> fondos, AnimatorController controller, Sprite primerFrame)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Cámara
        var camGO = new GameObject("Main Camera");
        camGO.tag = "MainCamera";
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 7f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.09f, 0.05f, 0.14f);
        camGO.transform.position = new Vector3(0f, 0f, -10f);
        camGO.AddComponent<AudioListener>();

        // Fondo con parallax (de lejos a cerca)
        var fondo = new GameObject("Fondo");
        CapaFondo(fondo.transform, "Cielo", fondos["skyline-a"], 0.9f, -1f, -30);
        CapaFondo(fondo.transform, "Edificios lejanos", fondos["buildings-bg"], 0.7f, -5f + fondos["buildings-bg"].bounds.size.y / 2f, -20);
        CapaFondo(fondo.transform, "Edificios cercanos", fondos["near-buildings-bg"], 0.4f, -5f + fondos["near-buildings-bg"].bounds.size.y / 2f, -10);

        // Suelo y plataformas
        var nivel = new GameObject("Nivel");
        Bloque(nivel.transform, "Suelo", suelo, new Vector2(20f, -9f), new Vector2(70f, 8f));
        Bloque(nivel.transform, "Plataforma 1", suelo, new Vector2(8f, -3.5f), new Vector2(5f, 1f));
        Bloque(nivel.transform, "Plataforma 2", suelo, new Vector2(15f, -1.5f), new Vector2(5f, 1f));
        Bloque(nivel.transform, "Plataforma 3", suelo, new Vector2(22f, -3.5f), new Vector2(5f, 1f));
        Bloque(nivel.transform, "Pared izquierda", suelo, new Vector2(-15.5f, 0f), new Vector2(1f, 20f));

        // Jugador
        var player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = new Vector3(0f, -2.5f, 0f);
        var sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = primerFrame;
        sr.sortingOrder = 10;

        var rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sharedMaterial = MaterialSinFriccion();

        var cap = player.AddComponent<CapsuleCollider2D>();
        cap.size = new Vector2(0.9f, 3.1f);
        cap.offset = new Vector2(0f, -0.55f);

        var anim = player.AddComponent<Animator>();
        anim.runtimeAnimatorController = controller;

        player.AddComponent<PlayerController>();

        PrefabUtility.SaveAsPrefabAssetAndConnect(player, "Assets/Prefabs/Player.prefab", InteractionMode.AutomatedAction);

        // La cámara sigue al jugador
        var follow = camGO.AddComponent<CameraFollow>();
        follow.objetivo = player.transform;
        camGO.transform.position = player.transform.position + follow.desplazamiento;

        string scenePath = "Assets/Scenes/Nivel1.unity";
        EditorSceneManager.SaveScene(scene, scenePath);

        var escenas = EditorBuildSettings.scenes.Where(s => s.path != scenePath).ToList();
        escenas.Insert(0, new EditorBuildSettingsScene(scenePath, true));
        EditorBuildSettings.scenes = escenas.ToArray();
    }

    static void CapaFondo(Transform padre, string nombre, Sprite s, float factor, float y, int orden)
    {
        var go = new GameObject(nombre);
        go.transform.SetParent(padre);
        go.transform.position = new Vector3(0f, y, 0f);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = s;
        sr.drawMode = SpriteDrawMode.Tiled;
        float ancho = s.bounds.size.x;
        int repeticiones = Mathf.CeilToInt(140f / ancho);
        sr.size = new Vector2(ancho * repeticiones, s.bounds.size.y);
        sr.sortingOrder = orden;
        var p = go.AddComponent<Parallax>();
        p.factor = factor;
    }

    static void Bloque(Transform padre, string nombre, Sprite s, Vector2 pos, Vector2 tam)
    {
        var go = new GameObject(nombre);
        go.transform.SetParent(padre);
        go.transform.position = pos;
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = s;
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = tam;
        var bc = go.AddComponent<BoxCollider2D>();
        bc.size = tam;
    }

    static PhysicsMaterial2D MaterialSinFriccion()
    {
        string path = "Assets/Art/SinFriccion.physicsMaterial2D";
        var m = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(path);
        if (m == null)
        {
            m = new PhysicsMaterial2D("SinFriccion") { friction = 0f, bounciness = 0f };
            AssetDatabase.CreateAsset(m, path);
        }
        return m;
    }
}
