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
| Salir (en menús) | Esc |

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
| `CamaraSigueJugador` | La cámara sigue al jugador de forma suave (`Lerp`) |
| `Dron` | Enemigo que patrulla; hace daño al tocarlo; explota si le disparas o lo pisas |
| `Peligro` | Hace daño al tocarlo (charcos y láseres) |
| `Laser` | Enciende y apaga el láser cada cierto tiempo |
| `Credito` | Moneda: suma puntos |
| `ChipSalto` / `Bateria` | Power-ups: super salto / +1 vida |
| `Terminal` | Meta del nivel: carga la siguiente escena |
| `Puntuacion` | Guarda los puntos entre escenas (`static`) |
| `Marcador` | Muestra vida y puntos en pantalla |
| `Pantalla` | Menú, Victoria y Derrota |

## 🎞️ Animator
- **Jugador** (`Animations/Player/Player.controller`): Idle, Run, Jump, Disparar y Herido. Parámetros bool `Moviendo`, `Saltando`, `Atacando`, `Herido`. La animación **Disparar** tiene dos eventos: `Disparo` (hace el daño) y `FinAtaque`.
- **Dron** (`Animations/Dron/Dron.controller`): Volar → Explotar (parámetro `Muerto`). La animación **Explotar** tiene el evento `Destruir`.

## 🎬 Escenas
`Menu` → `Nivel1` → `Nivel2` → `Victoria` (o `Derrota` si te quedas sin vidas).

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
