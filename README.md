# NEON RUN

Videojuego de plataformas 2D hecho en Unity para el módulo de Multimedia (2º DAM, primer trimestre).

Autores: Víctor Ucedo, (nombre del compañero)

## Descripción

Kai huye de la corporación OmniCorp a lo largo de dos niveles. Hay que llegar al terminal del final de cada nivel sin perder las 3 vidas.

- Nivel 1: drones, charcos electrificados y agujeros.
- Nivel 2: drones más rápidos, láseres intermitentes, charcos y agujeros.
- Power-ups: chip de super salto (8 s) y batería (+1 vida).
- Puntuación: crédito +10, dron destruido +50, terminal +100. El récord se guarda con `PlayerPrefs`.

## Controles

| Acción | Tecla |
|--------|-------|
| Moverse | A / D o flechas |
| Saltar | Espacio |
| Disparar | J |
| Empezar / continuar | Enter |
| Pausa / salir en menús | Esc |
| Volver al menú (en pausa) | M |

## Requisitos

- Unity 2022.3.62f3 (los dos con la misma versión).
- URP 14 (incluido en `Packages/manifest.json`).

## Abrir el proyecto

1. `git clone https://github.com/victorucedo/videojuego-multimedia-dam.git`
2. Unity Hub > Add > Add project from disk > seleccionar la carpeta.

## Escenas

`Menu` > `Intro` > `Nivel1` > `Nivel2` > `Victoria`. Si te quedas sin vidas, `Derrota`.

## Scripts

| Script | Función |
|--------|---------|
| `ControlJugador` | Movimiento, salto, disparo, animaciones, vida, daño y power-ups del jugador |
| `Dron` | Enemigo que patrulla; explota si le disparas o lo pisas |
| `Peligro` | Hace daño al tocarlo (charcos y láseres) |
| `Laser` | Enciende y apaga el láser cada cierto tiempo |
| `Credito`, `ChipSalto`, `Bateria` | Objetos que se recogen |
| `Checkpoint` | Guarda el punto de reaparición |
| `Terminal` | Final del nivel |
| `Puntuacion` | Puntos de la partida (`static`) |
| `Sonido` | Reproduce efectos de sonido (`static`) |
| `Marcador` | HUD: vida y créditos |
| `Pantalla` | Menú, Victoria y Derrota |
| `Intro` | Texto de la historia antes del Nivel 1 |
| `Pausa` | Pausa con `Time.timeScale` |
| `Fundido` | Fundido a negro entre escenas |
| `CamaraSigueJugador` | Cámara que sigue al jugador y tiembla al recibir daño |
| `Parallax` | Movimiento de las capas del fondo |
| `Parpadeo` | Carteles que parpadean |
| `Vehiculo` | Coches del fondo |

## Ejecutable

Se genera en `Build/` (no se sube al repositorio). Instrucciones de juego en `docs/LEEME.txt`.

## Assets

| Asset | Autor | Licencia |
|-------|-------|----------|
| [Warped City](https://ansimuz.itch.io/warped-city) (gráficos y música) | ansimuz, música de Pascal Belisle | CC0 |
| [Sci-Fi Sounds](https://www.kenney.nl/assets/sci-fi-sounds) | Kenney | CC0 |
| [Orbitron](https://fonts.google.com/specimen/Orbitron) | Matt McInerney | SIL Open Font License |

Los sonidos de lluvia y del zumbido de los charcos están generados para el proyecto.
