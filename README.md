# 🧩 KNU_CAPSTONE_DESIGN: Maze Soulslike 3D Game

🇰🇷 [한국어](#게임-소개-korean) | 🌐 [English](#about-the-game)

---

## 🌐 About the Game

**Maze Soulslike** is a 3D action-adventure game developed as part of the 2025 Capstone Design project at **Kangwon National University** by **Team SoulsStudio**.  
It draws inspiration from *Soulslike combat mechanics* and *maze-based exploration*.

You play as a mysterious warrior trapped in an ancient labyrinth. Navigate deadly corridors, defeat aggressive goblins, and survive with limited health resources. Every decision counts.

---

## 🔥 Features

- 🎮 Third-person combat with animation-driven movement
- 🧠 Enemy AI using FSM (Finite State Machine)
- 🧙 Patrol → Chase → Attack → Die behaviors
- 👀 Vision system using field-of-view cone + raycasting
- ⚔️ Combat system with timed attacks, hit colliders, and damage values
- 💉 Player heals on enemy defeat
- 💀 Goblin ragdoll physics on death
- 🔊 Randomized patrol and attack SFX
- 🪧 Visual exclamation alert when player is detected
- 📜 Modular & clean C# structure for reusability and expansion

---

## 🎓 Educational Focus

This project helped us apply and deepen knowledge in:

- Unity’s **NavMeshAgent** pathfinding
- **Animator Controller** logic and root motion
- AI pattern design using **FSM**
- Real-time **collision detection** and attack response
- Object-oriented game programming in **C#**
- Team-based development with **Git & GitHub**
- Scene setup, asset management, and **prefab systems**
- Sound design and **visual feedback systems**

---

## 🧩 Code Architecture

| Module                    | Purpose                                     |
|---------------------------|---------------------------------------------|
| `GoblinAI.cs`            | Controls goblin behavior FSM (state logic)  |
| `GoblinWeaponHandler.cs` | Manages attack collider triggers            |
| `GoblinVision.cs`        | Checks player visibility (FOV + Raycast)    |
| `PlayerController.cs`    | Basic player health, damage, and healing    |
| `UIManager.cs`           | Handles feedback prompts                    |
| `GameManager.cs`         | Scene state, pause/retry UI (optional)      |

---

## 🎮 How to Play

- Use **WASD** to move and mouse to look around
- Get close to goblins, **time your attacks** wisely
- Avoid getting surrounded
- Goblins patrol, detect, chase, and attack on sight
- **Heal by defeating enemies**
- Continue deeper into the maze...

---

## 🕹 Controls

| Action        | Input        |
|---------------|--------------|
| Move          | `W`, `A`, `S`, `D` |
| Rotate Camera | Mouse        |
| Attack        | Left Click   |
| Dodge/Roll    | Spacebar (planned) |
| Pause         | ESC (optional) |

---

## 🧠 Goblin AI Logic

stateDiagram-v2
    Patrol --> Chase : 플레이어를 발견함
    Chase --> Attack : 공격 범위에 진입
    Attack --> Chase : 플레이어 도망감
    Chase --> Patrol : 플레이어 시야에서 사라짐
    Attack --> Dead : HP가 0 이하
---
## 🛠 Tech Stack

- Unity 2022.3+
- C# scripting
- NavMesh for AI pathfinding
- Mixamo animations
- Git + GitHub version control
- Modular component design

---

## 🗂 Folder Structure

Assets/
├── Animations/         # 애니메이션 클립 및 컨트롤러
├── Audio/              # 사운드 효과 (발자국, 공격 등)
├── Materials/          # 머티리얼 및 셰이더
├── Models/             # 3D 모델 파일 (FBX 등)
├── Prefabs/            # 프리팹 (고블린, 플레이어, UI 등)
├── Scripts/
│   ├── Enemy/          # 고블린 AI, 무기 처리 스크립트
│   ├── Player/         # 플레이어 조작 및 전투 로직
│   └── UI/             # 체력 바, 게임 UI 관련 스크립트
├── Scenes/             # 게임 씬
├── UI/                 # UI 캔버스 구성 요소
└── Textures/           # 텍스처, 시각 효과용 이미지


---
## 👥 Team SoulsStudio (KNU)

| Name    | Role                |
| ------- | ------------------- |
| Farhana | Goblin              |
| Giho    | Skeleton            |
| Seunggon| Player              |
| Heedo   | Final Boss          |

---

📄 License
This project is intended for academic use only under the 2025 Kangwon National University Capstone Design Program.
All non-original assets (e.g., animations) are credited to their sources (e.g., Mixamo).

---

게임 소개 (Korean)
🎮 미로-소울라이크 3D 게임
Maze Soulslike는 강원대학교 2025 캡스톤디자인 수업의 결과물로, 팀 SoulsStudio가 제작한 액션 게임입니다.
플레이어는 고대 미궁에 갇힌 전사로, 감시하는 고블린들을 피해 싸우고 탈출해야 합니다.

✨ 게임 요소
FSM 기반 고블린 인공지능 (순찰 → 추적 → 공격 → 사망)
공격/회피 시스템과 HP 회복 로직
시야 감지 (각도 + 거리 기반)
루트 모션 애니메이션 제어
히트 이펙트, 사운드, 시각적 경고 등

즐겁게 플레이해 주세요! 🎮
팀 SoulsStudio 드림.
