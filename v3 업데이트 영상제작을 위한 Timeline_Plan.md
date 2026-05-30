# Timeline 구성 계획

## Chat GPT를 활용한 계획 작성

목적: GitHub 포트폴리오용 Update v3 기능 시연 영상 제작

공통 촬영 규칙:
- 각 영상은 10초 이내로 구성한다.
- 영상 안에는 제목이나 자막을 넣지 않는다.
- UI 문구와 DOTween 애니메이션은 이미 구현된 것을 그대로 사용한다.
- UI가 나타나거나 사라지는 순간에는 플레이어/카메라 움직임을 줄여 화면을 안정적으로 유지한다.
- 가능하면 60fps로 녹화한다.
- 영상은 아래 순서로 각각 따로 내보낸다.
  1. Global Volume 공포 분위기 개선
  2. 시작 키 설명 UI & DOTween 애니메이션
  3. Puzzle 2 키 설명 UI & DOTween 애니메이션
  4. Puzzle 4 UI 추가

## 01_GlobalVolume_공포분위기

목표 길이: 12.5초

시점: 연출형 느린 카메라 이동

Timeline 트랙:
- Cinemachine Track: 맵 분위기를 보여주는 느린 이동 카메라
- Animation Track: `Before` Virtual Camera 또는 VCam Rig 이동
- Animation Track: `After` Virtual Camera 또는 VCam Rig 이동
- Activation Track: 개선 전 조명/Volume 상태
- Activation Track: 개선 후 Global Volume 상태
- Activation Track: 우하단 `Before` 텍스트 UI
- Activation Track: 우하단 `After` 텍스트 UI
- Timing notes: 컷 전환 및 촬영 타이밍 확인용

구성:

| 시간 | 내용 |
| --- | --- |
| 0.0 - 3.0 | 개선 전 상태를 경로1의 느린 카메라 이동으로 보여준다. |
| 3.0 - 6.0 | 개선 전 상태를 경로2의 느린 카메라 이동으로 보여준다. |
| 6.0 | 컷 전환한다. 페이드는 사용하지 않는다. |
| 6.0 - 9.0 | 개선 후 상태를 경로1의 느린 카메라 이동으로 보여준다. 어두워진 조명, 낮아진 노출, 차갑거나 탁해진 색감을 강조한다.|
| 9.0 - 12.0 | 개선 후 상태를 경로2의 느린 카메라 이동으로 보여준다. 어두워진 조명, 낮아진 노출, 차갑거나 탁해진 색감을 강조한다.|
| 12.0 - 12.5 | 개선 후 화면을 0.5초 정도 유지한다. |

연출 메모:
- Before와 After는 같은 장소와 비슷한 카메라 경로를 사용한다.
- 카메라 이동은 느리게 잡아 색감과 노출 변화가 더 잘 보이게 한다.
- 벽, 바닥, 소품, 광원이 함께 보이는 구도를 선택하면 변화가 명확하다.
- 이 영상에는 UI를 넣지 않는 것이 좋다.
- 단, 전후 비교를 명확히 하기 위해 우하단에 작은 `Before`/`After` 텍스트 UI를 넣는다.
- 텍스트는 화면을 가리지 않게 작고 단순하게 배치한다.

Unity 설정 추천:
- 필요한 경우 Virtual Camera를 2개 만든다.
  - `VCam_GlobalVolume_Before`
  - `VCam_GlobalVolume_After`
- 두 카메라는 같은 이동 경로 또는 거의 같은 이동 경로를 사용한다.
- 추천 이동 방식:
  - 복도를 아주 천천히 밀고 들어가는 카메라
  - 방 안을 느리게 훑는 카메라
  - 광원과 벽면이 함께 보이는 짧은 이동
- Cinemachine Track에 아래처럼 클립을 배치한다.
  - Before 카메라: 0.0 - 6초
  - After 카메라: 6 - 12.5초
- `VCam Before slow move animation` 트랙에는 `VCam_GlobalVolume_Before` 또는 그 부모 Rig의 Animator를 바인딩한다.
- `VCam After slow move animation` 트랙에는 `VCam_GlobalVolume_After` 또는 그 부모 Rig의 Animator를 바인딩한다.
- 생성되는 기본 이동 애니메이션:
  - Before: 0.0 - 3.0초 동안 로컬 x축으로 약 -7 이동
  - Before: 2번째 시작 위치 이동 후 3.0 - 6.0초 동안 로컬 z축으로 약 +7 이동
  - After: 6.0 - 9.0초 동안 로컬 x축으로 약 +7 이동
  - After: 2번째 시작 위치 이동 후 9.0 - 12.0초 동안 로컬 Z축으로 약 +7 이동
  - After final hold: 12.0 - 12.5초 동안 마지막 위치 유지
- 공포 분위기용 Global Volume이 별도 GameObject라면 6.0초부터 Activation Track으로 켠다.
- Canvas 안에 우하단 텍스트 UI를 2개 만든다.
  - `UI_GlobalVolume_BeforeLabel`: 텍스트 `Before`
  - `UI_GlobalVolume_AfterLabel`: 텍스트 `After`
- 두 UI는 RectTransform Anchor를 오른쪽 아래로 설정한다.
- 권장 위치:
  - Anchor: Bottom Right
  - Pivot: Bottom Right
  - Pos X: -40
  - Pos Y: 32
- Timeline 바인딩:
  - `Before label UI - bottom right`: `UI_GlobalVolume_BeforeLabel`
  - `After label UI - bottom right`: `UI_GlobalVolume_AfterLabel`
- `Before` UI는 0.0 - 6.0초만 켜지고, `After` UI는 6.0 - 12.5초만 켜지게 한다.

## 02_시작키_UI

목표 길이: 약 10초, UI가 완전히 사라지는 순간까지

시점: 플레이어 시점

Timeline 트랙:
- Cinemachine Track 또는 플레이어 카메라 촬영 트랙
- Timing notes: 플레이 버튼 클릭 및 UI 페이드아웃 타이밍 확인용
- 필요 시 Audio Track: UI 또는 게임 피드백 사운드가 기능 설명에 도움이 될 때만 사용

구성:

| 시간 | 내용 |
| --- | --- |
| 0.0 - 0.5 | 시작 화면 또는 플레이 시작 직전 상태를 보여준다. |
| 0.5 - 1.0 | Play 버튼을 누르거나 게임 시작 조건을 실행한다. |
| 1.0 - 2.0 | 시작 키 UI가 생성되고 기존 DOTween 애니메이션으로 나타나는 장면을 보여준다. 이때 화면을 안정적으로 유지한다. |
| 2.0 - 9.5 | UI가 잘 보이도록 플레이어 시점을 유지한다. 약간의 시점 이동은 가능하지만 과하게 움직이지 않는다. |
| 9.5 - 10.0 | 10초 뒤 UI가 페이드아웃으로 사라지는 순간까지 촬영한다. |

연출 메모:
- UI가 처음 나타나는 순간에는 플레이어를 움직이지 않는다.
- 배경이 너무 복잡하지 않은 구도에서 UI가 잘 보이게 한다.
- UI 페이드아웃이 완전히 끝난 뒤 영상을 종료한다.

Unity 설정 추천:
- Play 버튼을 Timeline에서 직접 누르기 어렵다면 Play Mode에서 직접 클릭한 뒤 녹화한다.
- Timeline 안에서는 12.5초 지점을 시작/클릭 기준 마커처럼 사용한다.

## 03_Puzzle2_키_UI

목표 길이: 약 6.5초

시점: 플레이어 시점

Timeline 트랙:
- Cinemachine Track 또는 플레이어 카메라 촬영 트랙
- Timing notes: Puzzle 2 트리거 진입/이탈 타이밍 확인용

구성:

| 시간 | 내용 |
| --- | --- |
| 0.0 - 1.2 | 플레이어가 Puzzle 2 UI 트리거 구역으로 접근한다. |
| 1.2 - 2.0 | 트리거 구역에 진입하고 Puzzle 2 키 UI가 기존 DOTween 애니메이션으로 나타난다. |
| 2.0 - 4.2 | UI가 명확히 보이도록 잠깐 멈추거나 아주 천천히 움직인다. |
| 4.2 - 5.4 | 트리거 구역 밖으로 이동한다. |
| 5.4 - 6.5 | UI가 완전히 사라지는 순간까지 촬영한다. |

연출 메모:
- 트리거 구역에 들어가기 직전 속도를 줄인다.
- UI 애니메이션이 재생되는 동안에는 화면을 안정적으로 유지한다.
- 구역에서 벗어나는 동선이 명확해야 UI가 왜 사라지는지 이해하기 쉽다.

Unity 설정 추천:
- Timeline에 아래 타이밍을 기준으로 클립 또는 안내 마커를 둔다.
  - 25.2초: Puzzle 2 UI 트리거 진입
  - 28.2초: Puzzle 2 UI 트리거 이탈
  - 30.5초: 녹화 종료

## 04_Puzzle4_UI

목표 길이: 약 7.5초

시점: 플레이어 시점

Timeline 트랙:
- Cinemachine Track 또는 플레이어 카메라 촬영 트랙
- Timing notes: Puzzle 4 UI 표시/숨김 트리거 타이밍 확인용

구성:

| 시간 | 내용 |
| --- | --- |
| 0.0 - 1.0 | 플레이어가 Puzzle 4 UI 표시 트리거 경로로 접근한다. |
| 1.0 - 2.0 | 첫 번째 트리거 구역을 지나치며 Puzzle 4 UI가 나타난다. |
| 2.0 - 4.5 | 추가된 UI가 잘 보이도록 시점을 안정적으로 유지한다. |
| 4.5 - 6.2 | UI를 숨기는 두 번째 구역으로 이동한다. |
| 6.2 - 7.5 | 두 번째 구역에 들어가고 UI가 완전히 사라지는 순간까지 촬영한다. |

연출 메모:
- 첫 번째 트리거는 플레이어가 자연스럽게 길을 지나가다가 발생하는 느낌으로 잡는다.
- UI가 나타난 직후에는 바로 움직이지 말고 짧게 유지한다.
- UI가 사라진 것을 확인한 뒤 영상을 종료한다.

Unity 설정 추천:
- Timeline에 아래 타이밍을 기준으로 클립 또는 안내 마커를 둔다.
  - 33.0초: Puzzle 4 UI 표시 트리거 통과
  - 38.2초: Puzzle 4 UI 숨김 트리거 진입
  - 39.5초: 녹화 종료

## Timeline 에셋 구성 방식

기존 `UpdateDirectorTimeline.playable`을 사용한다.

권장 구성:
- 하나의 Master Timeline 안에 4개 섹션을 시간 간격을 두고 배치한다.
- 각 섹션을 따로 녹화하거나, 전체 녹화 후 구간별로 잘라낸다.

섹션 시간:

| 섹션 | 시간 범위 |
| --- | --- |
| Global Volume 공포 분위기 | 0.0 - 12.5 |
| 시작 키 UI | 12.5 - 22.0 |
| Puzzle 2 UI | 24.0 - 30.5 |
| Puzzle 4 UI | 32.0 - 39.5 |

GitHub에는 각 섹션을 독립된 짧은 영상으로 올린다.
