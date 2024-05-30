# 3DUnityAdvancedProject

## 3D 플랫포머 제출용 개인과제입니다.

#### 기간 2024.05.28 ~ 2024.05.30 (추후 리팩토링 예정)

#### 2024.05.30 기준 구현 내용
<br/> 플레이어 움직임 : WASD
<br/> 좌클릭 공격
<br/> 우클릭 누르고 유지하며 좌우 이동하여 카메라 조정
<br/> 스페이스바 : 점프
<br/> 왼쪽 Shift : 달리기

#### 2024.05.30 기준 문제점
<br/> 플레이어가 달리기를 해도 UI에 줄어드는 스태미나가 업데이트되지 않음
<br/> 점프를 하고 착지를 했을 때 다시 점프가 되지 않거나 끼는 현상

#### 프로젝트 목적 : StateMachine, Observer 디자인 패턴 등을 적용하여 간단한 3D 플랫포머 구현


참고 영상 :  https://www.youtube.com/watch?v=--_CH5DYz0M&t=216
</br>사용 에셋 :  https://github.com/KyleBanks/scene-ref-attribute
</br>https://assetstore.unity.com/packages/3d/environments/poly-style-platformer-starter-pack-284167
</br>https://assetstore.unity.com/packages/3d/characters/humanoids/rpg-tiny-hero-duo-pbr-polyart-225148
