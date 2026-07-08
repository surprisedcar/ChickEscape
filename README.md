# 🐥 Chick Escape

> 쿠키런 스타일의 2D 무한 러닝 액션 게임

---

## 🎮 게임 소개

<p align="center">
  <img src="ChickenEscpaegif2.gif" width="700">
</p>
플레이어가 **알 → 병아리 → 닭**으로 성장하며
점점 더 어려운 장애물을 극복하는 무한 러닝 게임.

---

## ✨ 주요 특징

- 🌾 모이를 먹으며 성장
- 🥚 알 → 🐤 병아리 → 🐔 닭 진화
- 🏃 슬라이딩 / 점프 / 2단 점프
- ❤️ 체력 시스템
- ⭐ 점수 시스템

---

## 🥚 성장 시스템

| 단계 | 사용 가능한 행동 | 새로운 장애물 | 진화 조건 |
|------|----------------|--------------|-----------|
| 🥚 알 | 1단 점프, 슬라이딩 | 🐍 뱀 | 모이 20개 + 점프 후 착지를 통한 알깨기 |
| 🐤 병아리 | 1단 점프, 슬라이딩 | 🪨 돌 | 모이 20개 |
| 🐔 닭 | 2단 점프, 1단 점프, 슬라이딩 | 🌳 나무 | 최종 단계 |

---

## ❤️ 게임 오버

체력이 모두 소진되면 게임이 종료.
특히 알 단계에서는 모이를 다 먹기 전에 알이 꺠지면 빠르게 게임 오버.

최종 화면에는

- 🎉최고 점수
- ⭐ 점수
- 🌾 획득한 모이 개수

가 표시된다.

---

## 🛠 개발 환경

- Engine : Unity 6
- Language : C#
- IDE : Visual Studio

---
## 📹 게임 플레이 영상

https://youtu.be/영상주소

---
## 💛 게임 다운로드
https://github.com/surprisedcar/ChickEscape/releases/download/v1.0/chickenEscapePlay.zip


---
## 📷 게임 화면

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/a585306a-af15-4fb5-8b86-baecf41d5a8b" />
시작 화면

<img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/4215c9a1-bc29-4448-aeb0-0a950062a636" />
매뉴얼(설명)화면

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/6a47f334-3c62-498b-8202-0dfbf23eb471" />
알 단계 화면. 먹은 모이 개수와 현재 score, 체력바가 표시된다. 배경은 알 공장.

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/97cf63ca-42b2-4f95-b702-06faf04bfbe4" />
병아리 단계 화면. 배경은 정글.

<img width="1915" height="1080" alt="image" src="https://github.com/user-attachments/assets/4110e8be-e3c2-464e-8e5b-ef8102a12570" />
닭 단계 화면. 배경은 들판.

<img width="1920" height="1080" alt="image" src="https://github.com/user-attachments/assets/7f1c9a48-0f92-439b-8b9b-4df122bc2cb0" />
최종 화면. 최고 점수, 현재 점수, 모은 모이 출력.

---
## 📂 프로젝트 구조
```
Assets/
├── Home/              # 시작 화면
│   └── GameStart.cs
├── ChickenStation/     # 3번째 스테이지 (닭 구간)
│   ├── ChickenWalkController.cs   # 캐릭터 이동/조작
│   ├── ChickenBackScroll.cs       # 배경 스크롤
│   ├── ChickenGroundScroll.cs     # 바닥 스크롤
│   ├── TreeMover.cs / TreeHit.cs  # 나무 장애물 이동 및 충돌 처리
│   └── ChickenImage/              # 캐릭터 스프라이트
├── ChickStation/       # 2번째 스테이지 (병아리 구간)
│   ├── ChickWalkControl.cs
│   ├── ChickBackScroll.cs / ChickGroundScroll.cs
│   ├── RockSpawner.cs / RockMover.cs / RockHit.cs  # 돌 장애물 스폰 및 충돌
│   └── ChickImage/
├── EggStation/         # 1번째 스테이지 (알 구간)
│   ├── GameManager.cs
│   ├── Egg/
│   │   ├── EggController.cs
│   │   └── EggInputController.cs
│   ├── Snake/
│   │   ├── SnakeSpawner.cs / SnakeMover.cs / SnakeHit.cs #뱀 장애물 스폰 및 충돌
│   └── background/
│       ├── BackgroundScroll1.cs
│       └── GroundScroll.cs
├── Score/              # 점수/아이템 시스템
│   ├── ScoreManager.cs
│   ├── ItemSpawner.cs / ItemMove.cs / Item.cs
│   └── SceveUIUtilizer.cs
├── Status/             # 체력 UI
│   └── Status.cs
├── Sound/              # 사운드 관리
│   ├── BackgroundMusic.cs
│   └── SceneBGMSetter.cs
├── GameOver/           # 게임 오버 화면
│   ├── GameOverManager.cs
│   └── GameOverDisplay.cs
├── Scenes/             # 씬 파일
│   ├── HomeScene.unity
│   ├── ChickenStationScene.unity
│   ├── ChickStation.unity
│   ├── EggStationScene.unity
│   ├── ManualScene.unity
│   └── GameOverScene.unity
└── Font/               # 커스텀 폰트 (BMJUA, 나눔손글씨)
```

---
## 🧩 핵심 시스템 설명

### 스테이지 진행 구조
게임은 다음 3단계 스테이지로 구성.
1.**EggStation** — 알 캐릭터로 뱀 장애물을 피하는 구간
2.**ChickStation** — 병아리 캐릭터로 뱀 장애물과 돌 장애물을 피하는 구간
3.**ChickenStation** — 닭 캐릭터로 뱀, 돌, 나무 장애물을 피하는 구간

각 스테이지는 `~BackScroll.cs` / `~GroundScroll.cs` 스크립트로 배경과 바닥을 무한 스크롤시키고, `~WalkController.cs`(또는 `WalkControl.cs`)로 캐릭터 조작을 처리.

### 장애물 시스템
- 스테이지별로 `Spawner` → `Mover` → `Hit` 3개 스크립트가 한 세트로 동작.
- 예: `RockSpawner.cs`(생성) → `RockMover.cs`(이동) → `RockHit.cs`(충돌 판정)

### 점수 및 아이템 (`Score/`)
- `ItemSpawner.cs`로 아이템(씨앗 등) 생성
- `ScoreManager.cs`에서 점수 누적 관리

### 체력 시스템 (`Status/Status.cs`)
- 하트 이미지 기반 체력바 UI 관리

### 사운드 (`Sound/`)
- `BackgroundMusic.cs`: 배경음악 재생
- `SceneBGMSetter.cs`: 씬 전환 시 BGM 자동 전환
