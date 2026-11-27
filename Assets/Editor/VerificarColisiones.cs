using UnityEngine; using UnityEditor; using UnityEngine.Tilemaps; using System.Text;
public static class VerificarColisiones {
  [MenuItem("Neon Run/Verificar colisiones (solo en Play)")]
  static void Run(){
    if(!Application.isPlaying){ Debug.LogError("[Verificar] Entra en Play primero"); return; }
    var tms = new System.Collections.Generic.List<Tilemap>();
    foreach(var n in new[]{"Suelo","Plataformas"}){ var g=GameObject.Find(n); if(g) tms.Add(g.GetComponent<Tilemap>()); }
    int mask = LayerMask.GetMask("Suelo");
    var sb=new StringBuilder(); int bad=0, total=0;
    for(int x=-15;x<=360;x++) for(int y=-16;y<=12;y++){
      bool tile=false; foreach(var t in tms) if(t.GetTile(new Vector3Int(x,y,0))!=null) tile=true;
      bool col = Physics2D.OverlapPoint(new Vector2(x+0.5f,y+0.5f), mask)!=null;
      total++;
      if(tile!=col){ bad++; if(bad<60) sb.AppendLine("x="+x+" y="+y+" tile="+tile+" collider="+col); }
    }
    string r="[Verificar] celdas="+total+" discrepancias="+bad+"\n"+sb;
    System.IO.File.WriteAllText("Copias_seguridad/verificacion_"+UnityEngine.SceneManagement.SceneManager.GetActiveScene().name+".txt",r);
    Debug.Log(r);
  }
}
