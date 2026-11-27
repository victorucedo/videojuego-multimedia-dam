#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Tilemaps;

// Reconstruye Nivel1 y Nivel2 partiendo de las copias originales (Copias_seguridad).
// Solo usa lo que ya existe: tile de suelo, BordeNeon y los prefabs del proyecto.
// Altura h: 0 = nivel del suelo original, cada +1 sube una casilla (la superficie esta en y = -5 + h).
// Fisica del jugador: salto max ~5.6, velocidad 8 -> saltos de seguridad: hueco <=5 en llano, <=4 subiendo 2-3.
public static class ConstruirNiveles
{
    const string guidSuelo = "50c7232ef05e4584c8b2355adf9c489b";
    const string guidNeon  = "0ee94d432d063c945a78efc7d1127801";
    const int ABAJO = -16;

    class Datos
    {
        public string nombre, siguiente;
        public int ancho;                 // x de la pared final
        public int[][] bloques;           // {x0, x1, h}  columnas macizas
        public int[][] losas;             // {x0, x1, h}  plataformas finas flotantes
        public float[][] drones, charcos, lasers, checkpoints, baterias, extras; // {x, h}
        public float[] chip, terminal;    // {x, h}
        public float patrulla, velDron;
    }

    static float Y(float h) { return -5f + h; }

    static Datos Nivel1()
    {
        return new Datos {
            nombre = "Nivel1", siguiente = "Nivel2", ancho = 292,
            bloques = new[] {
                new[]{-15,16,0}, new[]{17,20,1}, new[]{21,26,2}, new[]{38,46,1}, new[]{58,66,2}, new[]{67,70,3}, new[]{71,74,4},
                new[]{80,90,1}, new[]{109,121,2}, new[]{127,131,3}, new[]{135,138,1}, new[]{157,168,2}, new[]{174,178,0},
                new[]{179,181,1}, new[]{182,184,2}, new[]{185,187,3}, new[]{192,201,2}, new[]{220,226,3}, new[]{232,236,2},
                new[]{248,255,2}, new[]{275,291,2} },
            losas = new[] {
                new[]{31,34,2}, new[]{51,53,3}, new[]{95,97,2}, new[]{102,105,3}, new[]{143,145,2}, new[]{150,152,3},
                new[]{206,208,3}, new[]{213,215,4}, new[]{241,243,3}, new[]{261,263,3}, new[]{268,270,2} },
            patrulla = 1.5f, velDron = 2.5f,
            drones = new[] { new[]{23f,2}, new[]{40f,1}, new[]{62f,2}, new[]{83f,1}, new[]{160f,2}, new[]{196f,2},
                             new[]{223f,3}, new[]{252f,2}, new[]{278f,2} },
            charcos = new[] { new[]{44f,1}, new[]{87f,1}, new[]{166f,2}, new[]{198.5f,2}, new[]{284f,2} },
            lasers = new float[0][],
            checkpoints = new[] { new[]{59.5f,2}, new[]{111f,2}, new[]{193f,2}, new[]{249.5f,2} },
            baterias = new[] { new[]{116f,2}, new[]{222f,3} },
            chip = new[]{ 120f, 2f },
            terminal = new[]{ 287f, 2f },
            extras = new[] { new[]{4f,0}, new[]{7f,0}, new[]{10f,0}, new[]{13f,0}, new[]{18.5f,1} }
        };
    }

    static Datos Nivel2()
    {
        return new Datos {
            nombre = "Nivel2", siguiente = "Victoria", ancho = 350,
            bloques = new[] {
                new[]{-15,12,0}, new[]{24,29,2}, new[]{40,47,3}, new[]{53,56,1}, new[]{76,87,3}, new[]{91,94,4},
                new[]{96,121,0}, new[]{122,124,1}, new[]{125,127,2}, new[]{128,137,3}, new[]{163,175,2}, new[]{181,184,1},
                new[]{189,192,3}, new[]{204,214,3}, new[]{220,222,2}, new[]{223,225,3}, new[]{226,228,4}, new[]{247,258,2},
                new[]{285,292,2}, new[]{298,300,3}, new[]{312,318,2}, new[]{330,349,2} },
            losas = new[] {
                new[]{17,19,1}, new[]{33,35,4}, new[]{61,64,2}, new[]{69,71,3}, new[]{98,100,5}, new[]{104,106,5},
                new[]{110,112,5}, new[]{116,121,5}, new[]{142,144,4}, new[]{149,151,5}, new[]{156,158,4}, new[]{197,199,4},
                new[]{233,235,4}, new[]{240,242,3}, new[]{264,266,3}, new[]{271,273,4}, new[]{278,280,3}, new[]{305,307,4},
                new[]{323,325,3} },
            patrulla = 1.5f, velDron = 3f,
            drones = new[] { new[]{26f,2}, new[]{80f,3}, new[]{100f,0}, new[]{115f,0}, new[]{132f,3}, new[]{207f,3},
                             new[]{250f,2}, new[]{314f,2}, new[]{334f,2} },
            charcos = new[] { new[]{84f,3}, new[]{104f,0}, new[]{113f,0}, new[]{174f,2}, new[]{211.5f,3}, new[]{253f,2},
                              new[]{316f,2}, new[]{344f,2} },
            lasers = new[] { new[]{45f,3}, new[]{109f,0}, new[]{170f,2}, new[]{289f,2}, new[]{341f,2} },
            checkpoints = new[] { new[]{41f,3}, new[]{77f,3}, new[]{129.5f,3}, new[]{164f,2}, new[]{205f,3},
                                  new[]{248.5f,2}, new[]{331f,2} },
            baterias = new[] { new[]{167f,2}, new[]{257f,2}, new[]{314f,2} },
            chip = new[]{ 105f, 5f },
            terminal = new[]{ 347f, 2f },
            extras = new[] { new[]{4f,0}, new[]{7f,0}, new[]{10f,0} }
        };
    }

    [MenuItem("Neon Run/Construir niveles 1 y 2")]
    public static void Ejecutar()
    {
        if (EditorApplication.isPlaying) { Debug.LogError("[Construir] Para el modo Play primero."); return; }
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity", OpenSceneMode.Single);
        string raiz = Directory.GetParent(Application.dataPath).FullName;
        foreach (string n in new[] { "Nivel1", "Nivel2" })
        {
            string origen = Path.Combine(raiz, "Copias_seguridad", n + "_original.unity");
            if (!File.Exists(origen)) { Debug.LogError("[Construir] Falta " + origen); return; }
            File.Copy(origen, Path.Combine(raiz, "Assets", "Scenes", n + ".unity"), true);
            AssetDatabase.ImportAsset("Assets/Scenes/" + n + ".unity", ImportAssetOptions.ForceUpdate);
        }
        AssetDatabase.Refresh();

        Construir(Nivel1());
        Construir(Nivel2());
        EditorSceneManager.OpenScene("Assets/Scenes/Nivel1.unity", OpenSceneMode.Single);
        AssetDatabase.SaveAssets();
        Debug.Log("[Construir] LISTO: Nivel1 y Nivel2 construidos.");
    }

    static void Construir(Datos d)
    {
        string ruta = "Assets/Scenes/" + d.nombre + ".unity";
        var escena = EditorSceneManager.OpenScene(ruta, OpenSceneMode.Single);

        GameObject suelo = GameObject.Find("Suelo");
        GameObject neon = GameObject.Find("Neon");
        GameObject fachada = GameObject.Find("Fachada");
        GameObject nivel = GameObject.Find("Nivel");
        if (suelo == null || neon == null || nivel == null) { Debug.LogError("[Construir] Falta Suelo/Neon/Nivel en " + ruta); return; }
        Tilemap tmSuelo = suelo.GetComponent<Tilemap>();
        Tilemap tmNeon = neon.GetComponent<Tilemap>();
        Transform grid = suelo.transform.parent;

        GameObject abismos = GameObject.Find("Abismos");
        GameObject plantilla = null;
        if (abismos != null && abismos.transform.childCount > 0)
            plantilla = Object.Instantiate(abismos.transform.GetChild(0).gameObject);

        Borrar<Dron>(); Borrar<Peligro>(); Borrar<Laser>(); Borrar<Checkpoint>();
        Borrar<ChipSalto>(); Borrar<Bateria>(); Borrar<Credito>(); Borrar<Terminal>();
        if (abismos != null) while (abismos.transform.childCount > 0) Object.DestroyImmediate(abismos.transform.GetChild(0).gameObject);
        if (abismos == null) abismos = new GameObject("Abismos");

        tmSuelo.ClearAllTiles();
        tmNeon.ClearAllTiles();

        // Tilemaps extra: Plataformas (con colision, las losas flotantes) y Decoracion (sin colision)
        var pl = Object.Instantiate(suelo, grid); pl.name = "Plataformas";
        Tilemap tmPlat = pl.GetComponent<Tilemap>(); tmPlat.ClearAllTiles();
        var de = Object.Instantiate(neon, grid); de.name = "Decoracion"; de.GetComponent<Tilemap>().ClearAllTiles();

        TileBase tSuelo = AssetDatabase.LoadAssetAtPath<TileBase>(AssetDatabase.GUIDToAssetPath(guidSuelo));
        TileBase tNeon = AssetDatabase.LoadAssetAtPath<TileBase>(AssetDatabase.GUIDToAssetPath(guidNeon));

        // ---- Bloques de suelo (columnas macizas de altura variable) ----
        int off = 20;
        bool[] cubierto = new bool[d.ancho + off + 5];
        foreach (var b in d.bloques)
            for (int x = b[0]; x <= b[1]; x++)
            {
                cubierto[x + off] = true;
                for (int y = ABAJO; y <= -6 + b[2]; y++) tmSuelo.SetTile(new Vector3Int(x, y, 0), tSuelo);
            }
        for (int y = ABAJO; y <= 19; y++)
        {
            tmSuelo.SetTile(new Vector3Int(-16, y, 0), tSuelo);
            tmSuelo.SetTile(new Vector3Int(d.ancho, y, 0), tSuelo);
        }
        // ---- Plataformas finas con borde neon ----
        foreach (var l in d.losas)
            for (int x = l[0]; x <= l[1]; x++)
            {
                tmPlat.SetTile(new Vector3Int(x, -6 + l[2], 0), tSuelo);
                tmNeon.SetTile(new Vector3Int(x, -6 + l[2], 0), tNeon);
            }
        tmSuelo.CompressBounds(); tmPlat.CompressBounds(); tmNeon.CompressBounds();
        // ---- Regenerar colisiones desde los tiles actuales (si no, se queda la forma del nivel original) ----
        foreach (var tmr in new[] { tmSuelo, tmPlat })
        {
            var tcol = tmr.GetComponent<TilemapCollider2D>();
            var ccol = tmr.GetComponent<CompositeCollider2D>();
            if (tcol != null) { tcol.enabled = false; tcol.enabled = true; tcol.ProcessTilemapChanges(); EditorUtility.SetDirty(tcol); }
            if (ccol != null) { ccol.generationType = CompositeCollider2D.GenerationType.Synchronous; ccol.GenerateGeometry(); EditorUtility.SetDirty(ccol); }
            EditorUtility.SetDirty(tmr);
        }

        // ---- Borde neon en la cara superior de cada bloque (suelo visible = suelo solido) ----
        Tilemap tmDeco = de.GetComponent<Tilemap>();
        foreach (var bl in d.bloques)
            for (int x = bl[0]; x <= bl[1]; x++)
                tmDeco.SetTile(new Vector3Int(x, -6 + bl[2], 0), tNeon);
        tmDeco.CompressBounds();

        // ---- Fachada: solo sobre suelo al nivel del suelo; nunca sobre huecos ni sobre bloques elevados ----
        if (fachada != null)
        {
            Tilemap tf = fachada.GetComponent<Tilemap>();
            var origen = new Dictionary<int, TileBase[]>();
            for (int x = -12; x <= 122; x++)
            {
                var col = new TileBase[4];
                for (int y = -5; y <= -2; y++) col[y + 5] = tf.GetTile(new Vector3Int(x, y, 0));
                origen[x] = col;
            }
            tf.ClearAllTiles();
            foreach (var bl in d.bloques)
            {
                if (bl[2] != 0) continue;
                for (int x = bl[0]; x <= bl[1]; x++)
                {
                    int src = -12 + (((x + 12) % 135) + 135) % 135;
                    TileBase[] col;
                    if (!origen.TryGetValue(src, out col)) continue;
                    for (int y = -5; y <= -2; y++)
                        if (col[y + 5] != null) tf.SetTile(new Vector3Int(x, y, 0), col[y + 5]);
                }
            }
            tf.CompressBounds();
        }

        // ---- Farolas y carteles repetidos cada 125 casillas ----
        var farolas = GameObject.Find("Farolas");
        for (int k = 1; k <= 3; k++)
        {
            float dx = 125f * k;
            if (farolas != null)
            {
                var lista = new List<Transform>();
                foreach (Transform c in farolas.transform) lista.Add(c);
                foreach (var c in lista)
                    if (c.position.x < 125f && c.position.x + dx < d.ancho - 2)
                    {
                        var n = Object.Instantiate(c.gameObject, farolas.transform);
                        n.name = c.gameObject.name;
                        n.transform.position = c.position + new Vector3(dx, 0, 0);
                    }
            }
            var carteles = new List<Transform>();
            foreach (Transform c in nivel.transform) if (c.name.StartsWith("Cartel") && c.position.x < 125f) carteles.Add(c);
            foreach (var c in carteles)
                if (c.position.x + dx < d.ancho - 2)
                {
                    var n = (GameObject)Object.Instantiate(c.gameObject, nivel.transform);
                    n.name = c.gameObject.name;
                    n.transform.position = c.position + new Vector3(dx, 0, 0);
                }
        }

        // ---- La banda oscura de fondo llega justo hasta la cara superior del suelo (y=-5), ni una casilla mas ----
        var sf = GameObject.Find("SueloFondo");
        if (sf != null) { var pp = sf.transform.position; var sr0 = sf.GetComponent<SpriteRenderer>(); float alto = sr0 != null ? sr0.bounds.size.y : 16f; sf.transform.position = new Vector3(pp.x, -5f - alto / 2f, pp.z); }

        // ---- Abismos: un dibujo por cada tramo sin suelo ----
        var huecos = new List<int[]>();
        int ini = -1;
        for (int x = -15; x < d.ancho; x++)
        {
            bool vacio = !cubierto[x + off];
            if (vacio && ini < 0) ini = x;
            if (!vacio && ini >= 0) { huecos.Add(new[] { ini, x - 1 }); ini = -1; }
        }
        if (plantilla != null)
        {
            foreach (var h in huecos)
            {
                float ancho = h[1] - h[0] + 1;
                var a = Object.Instantiate(plantilla, abismos.transform);
                a.name = "Abismo";
                a.transform.position = new Vector3((h[0] + h[1] + 1) / 2f, -12f, 0);
                a.transform.localScale = Vector3.one;
                var sr = a.GetComponent<SpriteRenderer>();
                if (sr != null && sr.drawMode != SpriteDrawMode.Simple) sr.size = new Vector2(ancho, 14f);
                else a.transform.localScale = new Vector3(ancho, 1, 1);
            }
            Object.DestroyImmediate(plantilla);
        }

        // ---- Objetos jugables (todos sobre una superficie de altura h) ----
        Transform padre = nivel.transform;
        foreach (var o in d.drones)
        {
            var g = Poner("Dron", padre, o[0], Y(o[1]) + 2f);
            var dr = g.GetComponent<Dron>();
            dr.tiempoPatrulla = d.patrulla; dr.velocidad = d.velDron;
            PrefabUtility.RecordPrefabInstancePropertyModifications(dr);
        }
        foreach (var o in d.charcos) Poner("Charco", padre, o[0], Y(o[1]) + 0.15f);
        foreach (var o in d.lasers) Poner("Laser", padre, o[0], Y(o[1]) + 2f);
        foreach (var o in d.checkpoints) Poner("Checkpoint", padre, o[0], Y(o[1]) + 2.1f);
        foreach (var o in d.baterias) Poner("Bateria", padre, o[0], Y(o[1]) + 1.1f);
        Poner("ChipSalto", padre, d.chip[0], Y(d.chip[1]) + 1.4f);
        var term = Poner("Terminal", padre, d.terminal[0], Y(d.terminal[1]) + 1.4f);
        var tc = term.GetComponent<Terminal>();
        tc.siguienteEscena = d.siguiente;
        PrefabUtility.RecordPrefabInstancePropertyModifications(tc);

        // ---- Creditos: marcan el camino (huecos en el arco del salto, losas y tramos largos) ----
        var peligros = new List<float>();
        foreach (var o in d.charcos) peligros.Add(o[0]);
        foreach (var o in d.lasers) peligros.Add(o[0]);
        foreach (var o in d.extras) Poner("Credito", padre, o[0], Y(o[1]) + 1.6f);

        var sup = new List<int[]>();
        foreach (var b in d.bloques) sup.Add(b);
        foreach (var l in d.losas) sup.Add(l);
        sup.Sort((a, b) => a[0].CompareTo(b[0]));
        for (int i = 0; i + 1 < sup.Count; i++)
        {
            int vacioCeldas = sup[i + 1][0] - sup[i][1] - 1;
            if (vacioCeldas >= 2)
            {
                float cx = (sup[i][1] + 1 + sup[i + 1][0]) / 2f;
                Poner("Credito", padre, cx, Y(Mathf.Max(sup[i][2], sup[i + 1][2])) + 2.8f);
            }
        }
        foreach (var l in d.losas)
            Poner("Credito", padre, (l[0] + l[1] + 1) / 2f, Y(l[2]) + 1.6f);
        foreach (var b in d.bloques)
        {
            if (b[1] - b[0] + 1 < 9 || b[0] < 20) continue;
            float cx = (b[0] + b[1] + 1) / 2f;
            bool choca = false;
            foreach (float px in peligros) if (Mathf.Abs(px - cx) < 3f) choca = true;
            if (!choca) Poner("Credito", padre, cx, Y(b[2]) + 1.6f);
        }

        EditorSceneManager.MarkSceneDirty(escena);
        EditorSceneManager.SaveScene(escena);
        Debug.Log("[Construir] Listo " + ruta + " | huecos: " + huecos.Count);
    }

    static GameObject Poner(string prefab, Transform padre, float x, float y)
    {
        var p = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/" + prefab + ".prefab");
        var g = (GameObject)PrefabUtility.InstantiatePrefab(p, padre);
        g.transform.position = new Vector3(x, y, 0);
        return g;
    }

    static void Borrar<T>() where T : Component
    {
        foreach (var c in Object.FindObjectsOfType<T>(true))
        {
            if (c == null) continue;
            GameObject o = PrefabUtility.GetOutermostPrefabInstanceRoot(c.gameObject);
            if (o == null) o = c.gameObject;
            Object.DestroyImmediate(o);
        }
    }
}
#endif
