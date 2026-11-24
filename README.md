# 🌆 NEON RUN — Videojuego Multimedia (2º DAM)

Videojuego de **plataformas 2D** con temática **cyberpunk**, desarrollado en **Unity** para el módulo de **Multimedia** (2º DAM, primer trimestre).

## 👥 Autores
- Víctor Ucedo
- _(Nombre del compañero)_

## 🎮 Descripción
Año 2087. Kai, una repartidora con un implante robado, huye de la corporación **OmniCorp** desde los bajos fondos hasta la azotea de su torre.

- **Nivel 1 — Bajos fondos:** drones patrulla, charcos electrificados y agujeros.
- **Nivel 2 — Torre OmniCorp:** drones más rápidos, láseres que se encienden y apagan, charcos y agujeros.
- **Power-ups:** chip de super salto (8 s) y batería (+1 vida).
- **Victoria:** hackear el terminal al final de cada nivel. **Derrota:** perder las 3 vidas.
- **Ataque:** Kai dispara con la tecla J (los drones explotan).
- **Puntuación:** créditos (+10), destruir drones disparando o saltando encima (+50), terminal (+100).

## 🎮 Controles
| Acción | Tecla |
|--------|-------|
| Moverse | A / D o ← / → |
| Saltar | Espacio |
| Disparar | J |
| Empezar / continuar | Enter |
| Pausa (en el juego) / Salir (en menús) | Esc |
| Volver al menú (en pausa) | M |

## ✅ Requisitos de la entrega
- [ ] Documento de diseño (género, juegos analizados, definición del juego, assets referenciados)
- [ ] Personaje principal controlable con al menos 3 animaciones
- [ ] Al menos un enemigo y varias formas de recibir daño
- [ ] Algún power-up
- [ ] 2 niveles o pantallas distintos
- [ ] Condición de victoria y derrota, con puntuación
- [ ] Ejecutable .exe para Windows + TXT con instrucciones (minimanual)

## 🛠️ Tecnología
- **Motor:** Unity 2022.3.62f3
- **Lenguaje:** C#
- **Render:** URP 2D (Universal Render Pipeline) con luces 2D

> ⚠️ Los dos debemos usar **exactamente la misma versión de Unity** para evitar problemas al abrir el proyecto.

## 📁 Estructura
```
/            → Proyecto de Unity (Assets/, Packages/, ProjectSettings/)
/docs        → Documento de diseño, prompts usados y minimanual
```

## 🚀 Cómo abrir el proyecto
1. Clonar el repositorio:
   ```bash
   git clone <URL-del-repositorio>
   ```
2. Abrir **Unity Hub** → *Add* → *Add project from disk* → seleccionar la carpeta clonada.
3. La primera vez tardará un poco (Unity regenera la carpeta `Library/`).

## 📜 Scripts (`Assets/Scripts`)
| Script | Función |
|--------|---------|
| `ControlJugador` | Movimiento, salto (`AddForce`), suelo con `BoxCast` (capa **Suelo**), disparo con `BoxCast` (capa **Enemigo**) llamado desde un evento de animación, animaciones, vida, daño con empujón, caída al vacío y super salto |
| `CamaraSigueJugador` | La cámara sigue al jugador de forma suave (`Lerp`) y tiembla al recibir daño (`Random.insideUnitCircle`) |
| `Parallax` | Las capas del fondo se desplazan a distinta velocidad (`Mathf.Repeat`) para dar profundidad |
| `Dron` | Enemigo que patrulla; hace daño al tocarlo; explota si le disparas o lo pisas |
| `Peligro` | Hace daño al tocarlo (charcos y láseres) |
| `Laser` | Enciende y apaga el láser cada cierto tiempo (activa/desactiva su hijo `Rayo`) |
| `Credito` | Moneda: suma puntos y flota arriba y abajo (`Mathf.Sin`) |
| `ChipSalto` / `Bateria` | Power-ups: super salto / +1 vida |
| `Terminal` | Meta del nivel: carga la siguiente escena |
| `Checkpoint` | Antena: al tocarla guarda el punto de reaparición del jugador y enciende su luz |
| `Puntuacion` | Guarda los puntos entre escenas (`static`) |
| `Marcador` | HUD: vida en segmentos (la última en rojo) y créditos |
| `Pantalla` | Menú, Victoria y Derrota: texto, "Pulsa ENTER" parpadeando y récord con `PlayerPrefs` |
| `Intro` | Cuenta la historia antes del Nivel 1: las frases salen letra a letra (`Substring`); ENTER completa o pasa de frase |
| `Pausa` | ESC pausa el juego con `Time.timeScale = 0`; M vuelve al menú |
| `Fundido` | Imagen negra que se aclara al empezar cada escena y se oscurece antes de cambiar (`SceneManager.LoadScene`) |
| `Sonido` | Reproduce un efecto de sonido desde cualquier script (`static`, como `Puntuacion`) |

## 💡 Iluminación (URP 2D)
- **Luz Global** oscura y azulada en los niveles: el ambiente es de noche.
- **Luces puntuales** (Light 2D): sobre Kai, en el fogonazo del disparo, ojo rojo de los drones, láseres, charcos, créditos, power-ups, terminal y carteles de neón.
- **Farolas** que alternan cian y magenta a lo largo del nivel, creando zonas de luz y de sombra.
- El cielo y los edificios lejanos usan material *Sprite-Unlit* (no les afecta la luz); la capa de edificios más cercana usa *Sprite-Lit* para que se vean los focos de luz.

## 🧱 Niveles (Tilemap)
- Cada nivel tiene un objeto `Nivel` con un **Grid** y tres **Tilemaps**:
  - `Fachada`: muros y ventanas de fondo (solo decoración).
  - `Suelo`: el suelo y las plataformas, en la capa **Suelo**, con `TilemapCollider2D` + `CompositeCollider2D` (un solo collider para todo el suelo).
  - `Neon`: borde de neón cian encima de las plataformas (material *Unlit*, siempre brilla).
- Los tiles están en `Assets/Art/Tiles` (recortados del tileset de Warped City).
- **Checkpoint** a mitad de cada nivel: si caes al vacío reapareces en la última antena tocada.
- **Carteles de neón animados** (prefabs en `Prefabs/Carteles`, con su Animator y una luz 2D).
- **Lluvia**: sistema de partículas hijo de la cámara.
- Los agujeros del suelo tienen un fondo oscuro (`Abismos`) para que se vean como un precipicio.

## 🖥️ Interfaz
- Fuente **Orbitron** en toda la interfaz.
- **HUD**: panel con la vida en 3 segmentos y los créditos; arriba en el centro el sector y el objetivo; a la derecha los controles.
- **Menú** con el título en neón, panel de controles, "Pulsa ENTER" parpadeando y el récord.
- **Pausa** con ESC y **fundido a negro** entre escenas (así también da tiempo a que suene el terminal).
- **Récord** guardado con `PlayerPrefs` (se mantiene al cerrar el juego).

## ✨ Feedback visual
- **Partículas** (prefabs en `Prefabs/Particulas`, se crean con `Instantiate` y se borran solas al acabar): chispas al recoger créditos y power-ups, explosión del dron y polvo al aterrizar.
- **Temblor de cámara** cuando Kai pierde una vida.
- **Créditos flotando** y **parallax** en el fondo de los niveles.

## 🔊 Audio
- **Efectos** (con `Sonido.Reproducir(clip)`): salto, disparo, daño, explosión del dron, recoger crédito, power-ups y hackear el terminal.
- **Ambiente** distinto por zona: un objeto `Ambiente` con un `AudioSource` en bucle (sin código): zumbido grave de ciudad en el Nivel 1, zumbido industrial en el Nivel 2 y uno suave en los menús.

## 🎞️ Animator
- **Jugador** (`Animations/Player/Player.controller`): Idle, Run, Jump, Disparar y Herido. Parámetros bool `Moviendo`, `Saltando`, `Atacando`, `Herido`. La animación **Disparar** tiene dos eventos: `Disparo` (hace el daño) y `FinAtaque`.
- **Dron** (`Animations/Dron/Dron.controller`): Volar → Explotar (parámetro `Muerto`). La animación **Explotar** tiene el evento `Destruir`.

## 🎬 Escenas
`Menu` → `Intro` → `Nivel1` → `Nivel2` → `Victoria` (o `Derrota` si te quedas sin vidas).

## 📦 Ejecutable
El `.exe` se genera en la carpeta `Build/` (no se sube al repositorio). El minimanual está en `docs/LEEME.txt`.

## 🤝 Forma de trabajar
- Antes de empezar: `git pull`
- Al terminar: `git add .` → `git commit -m "Descripción del cambio"` → `git push`
- **No editar la misma escena a la vez** (las escenas dan conflictos). Mejor repartirse escenas o trabajar con prefabs.
- Cada uno puede trabajar en su propia rama y luego unir con un Pull Request.

## 📦 Assets utilizados
| Asset | Autor | Enlace | Licencia |
|-------|-------|--------|----------|
| Warped City | ansimuz | https://ansimuz.itch.io/warped-city | CC0 (música: crédito a Pascal Belisle) |
| Sci-Fi Sounds | Kenney | https://www.kenney.nl/assets/sci-fi-sounds | CC0 |
| Orbitron (fuente) | Matt McInerney | https://fonts.google.com/specimen/Orbitron | SIL Open Font License |
