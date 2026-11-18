# 🌆 NEON RUN — Videojuego Multimedia (2º DAM)

Videojuego de **plataformas 2D** con temática **cyberpunk**, desarrollado en **Unity** para el módulo de **Multimedia** (2º DAM, primer trimestre).

## 👥 Autores
- Víctor Ucedo
- _(Nombre del compañero)_

## 🎮 Descripción
Año 2087. Kai, una repartidora con un implante robado, huye de la corporación **OmniCorp** desde los bajos fondos hasta la azotea de su torre.

- **Nivel 1 — Bajos fondos:** drones patrulla, matones, charcos electrificados.
- **Nivel 2 — Torre OmniCorp:** torretas láser, láseres de seguridad, caídas.
- **Power-ups:** chip de doble salto, escudo de energía, batería (vida).
- **Victoria:** hackear el terminal al final del nivel. **Derrota:** perder las 3 vidas.
- **Puntuación:** créditos recogidos + bonus por tiempo y enemigos.

## 🎮 Controles
| Acción | Tecla |
|--------|-------|
| Moverse | A / D o ← / → |
| Saltar | Espacio |

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

## 🧰 Herramienta de configuración
`Assets/Editor/NeonRunSetup.cs` añade el menú **NEON RUN → Configurar personaje y nivel de prueba**, que regenera las animaciones del jugador, su Animator y la escena `Nivel1`. ⚠️ Volver a ejecutarlo **sobrescribe** `Nivel1`.

## 🤝 Forma de trabajar
- Antes de empezar: `git pull`
- Al terminar: `git add .` → `git commit -m "Descripción del cambio"` → `git push`
- **No editar la misma escena a la vez** (las escenas dan conflictos). Mejor repartirse escenas o trabajar con prefabs.
- Cada uno puede trabajar en su propia rama y luego unir con un Pull Request.

## 📦 Assets utilizados
| Asset | Autor | Enlace | Licencia |
|-------|-------|--------|----------|
| Warped City | ansimuz | https://ansimuz.itch.io/warped-city | CC0 (música: crédito a Pascal Belisle) |
