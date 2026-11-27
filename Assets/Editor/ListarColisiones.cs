using UnityEngine; using UnityEditor; using UnityEditor.SceneManagement; using System.Text;
public static class ListarColisiones {
  [MenuItem("Neon Run/Listar colisiones")]
  static void Run(){
    var sb=new StringBuilder();
    foreach(var n in new[]{"Nivel1","Nivel2"}){
      EditorSceneManager.OpenScene("Assets/Scenes/"+n+".unity",OpenSceneMode.Single);
      sb.AppendLine("== "+n);
      foreach(var c in Object.FindObjectsOfType<Collider2D>()){
        var b=c.bounds; var tm=c.GetComponent<UnityEngine.Tilemaps.Tilemap>();
        sb.AppendLine(c.gameObject.name+" | "+c.GetType().Name+" trig="+c.isTrigger+" layer="+LayerMask.LayerToName(c.gameObject.layer)+" x="+b.min.x.ToString("0.0")+".."+b.max.x.ToString("0.0")+" y="+b.min.y.ToString("0.0")+".."+b.max.y.ToString("0.0")+" used="+c.usedByComposite);
      }
    }
    System.IO.File.WriteAllText("Copias_seguridad/colisiones.txt",sb.ToString());
    Debug.Log("[Listar] hecho");
  }
}
