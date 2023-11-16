### 프로젝트 소개

- 내일배움개발 1조 HexaCore의 플레이어 A씨 게임 개발 도중 실행 중 버그가 발생했다.
- 발생한 버그를 해치우며 레벨업 하여 게임을 완성하자!

#### Login

![](https://i.imgur.com/bJ8efC3.jpg)

- Json을 이용한 Character Data Parsing
- 최초 케릭터 생성시 기본 Json 데이터를 생성
- ID 만으로 조회하여 게임 로드를 가능하도록 데이터 관리
- 아이디 입력 => 직업 선택 => 직업 보너스 배경 선택 등의 컨셉을 잡고 개발 진행

#### GameManager

![](https://i.imgur.com/8I0I3wX.jpg)

- Console ReadKey 입력은 예외처리에 대한 부분과 시각적인 부분에서 Arrow Function 키로 작업
- 정적 데이터 및 구조체 클래스 등을 이용한 클래스간의 의존서을 낮춰 협업시 문제가 없도록 설계
- Manager의 싱글톤으로 게임을 컨트롤

#### Character 

![](https://i.imgur.com/euolLwR.png)

- ASC를 이용한 Stauts UI 표현 시도
- 정적파일과 관리되는 아이템의 데이터를 로드하여 직렬화 및 역직렬화를 통해서 Json 객체 관리
- 추가적인 사항들은 이후 프로젝트 진행도에 따라서 추가될 예정

#### Store And Inventory

![](https://i.imgur.com/cLji40O.png)


- Json 객체를 이용한 Data 처리
- 아이템 리스트의 경우 Arrow Key 를 이용해서 스크롤 기능 구현
- 싱글톤 으로 데이터 관리
- 아이템 클래스의 경우 Dictionary 형식의 데이터를 ItemName을 Key로 Access

#### Battle

![](https://i.imgur.com/Q8CyQkI.png)

- 턴제 방식의 배틀 시스템 구현
- 턴 진행시 UI에 log 식의 데이터 처리

#### Reward

![](https://i.imgur.com/tqzeCIl.png)

- 전투 결과를 Bool 값으로 가져와 각 해당 조건에 따라서 보상진행
- 기본 시스템 개발 후 기타 Reward Item의 경우 개발시 추가할 예정

### Utility

#### Dummy Data
- 개발시 모든 품목 및 상태를 정할 수 없기 때문에 기초적인 Dummy Data 작성
- 케릭터 및 아이템의 기본 데이터는 Json 파일로 생성

![](https://i.imgur.com/WWvwKCg.png)
