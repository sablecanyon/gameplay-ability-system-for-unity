# EX Gameplay Ability System For Unity(W.I.P 文档还在施工中)
## 前言
该项目为Unreal Engine的Gameplay Ability System的Unity实现，目前实现了部分功能，后续会继续完善。

该项目完全开源，欢迎大家一起参与开发，提出建议，共同完善。可以基于该项目进行二次开发。
## ！！提醒！！请注意！！
__*该项目依赖Odin Inspector插件（付费），请自行解决!!!!!!!!*__

```若没有方法解决，可以加反馈qq群（616570103），群内提供帮助 ```

目前EX-GAS没有非常全面的测试，所以存在不可知的大量bug和性能问题。所以对于打算使用该插件的朋友请谨慎考虑，当然我是希望更多人用EX-GAS，毕竟相当于是变相的QA。

但是要讲良心嘛，现在的EX-GAS算不上很稳定的版本，如果你是业余时间开发rpg类的独立游戏，开发时间十分充裕，我当然建议你试试EX-GAS，我会尽可能修复bug，提供使用上的帮助。

如果有好兄弟确实打算用，那么请务必加反馈群（616570103）。我会尽力抽时间修复提出的bug。

>我非常希望EX-GAS能早日稳定，为更多游戏提供支持帮助。

## 参考案例 [Demo](Assets/Demo)

## 目录
- 1.[快速开始](#快速开始)
  - [安装](#安装)
  - [使用](#使用)
- 2.[GAS系统介绍](#GAS系统介绍)
  - [2.1 EX-GAS概述](#21-ex-gas概述)
  - [2.2 GameplayTag](#22-gameplaytag)
  - [2.3 Attribute](#23-attribute)
  - [2.4 AttributeSet](#24-attributeset)
  - [2.5 ModifierMagnitudeCalculation](#25-modifiermagnitudecalculation)
  - [2.6 GameplayCue](#26-gameplaycue)
  - [2.7 GameplayEffect](#27-gameplayeffect)
  - [2.8 Ability](#28-ability)
  - [2.9 AbilitySystemComponent](#29-abilitysystemcomponent)
- 3.[API(W.I.P 施工中)](#3apiwip)
- 4.[可视化功能](#4可视化功能)
  - [GAS Base Manager (GAS基础配置管理器)](#1-gas-base-manager-gas基础配置管理器)
  - [GAS Asset Aggregator (GAS配置资源聚合器)](#2-gas-asset-aggregator-gas配置资源聚合器)
  - [GAS Runtime Watcher (GAS运行时监视器)](#3-gas-runtime-watcher-gas运行时监视器)
- 5.[如果...我想...,应该怎么做?(持续补充)](#5如果我想应该怎么做wip)
- 6.[暂不支持的功能（可能有遗漏）](#6暂不支持的功能可能有遗漏)
- 7.[后续计划](#7后续计划)
- 8.[特别感谢](#8特别感谢)
- 9.[插件反馈渠道](#9插件反馈渠道)
## 1.快速开始
### 安装
1. 导入Odin Inspector插件(付费),Odin Inspector来源请自行解决。建议使用3.2+版本。
2. 导入本插件，建议以下两种方式：
- 使用Unity Package Manager安装
在Unity Package Manager中添加git地址:https://github.com/No78Vino/gameplay-ability-system-for-unity.git?path=Assets/GAS
>【国内镜像】https://gitee.com/exhard/gameplay-ability-system-for-unity.git?path=Assets/GAS
- 使用git clone本仓库[镜像同上]，然后将Assets/GAS文件夹拷贝到你的项目中即可

### 使用
GAS十分复杂，使用门槛较高。因为本项目是对UE的GAS的模仿移植，所以实现逻辑基本一致。建议先粗略了解一下UE版本的GAS整体逻辑，参考项目文档：https://github.com/BillEliot/GASDocumentation_Chinese

#### *使用流程*
1. 基础设置

在ProjectSetting中（或者Edit Menu栏入口：EX-GAS -> Setting），找到EX Gameplay Ability System的基本设置界面：

![S0X2@(E97LP_SWIJY2SJ@F3.png](Wiki%2FS0X2%40%28E97LP_SWIJY2SJ%40F3.png)

设置好以下两个路径
- 配置文件Asset路径: 这是该项目有关GAS的配置的路径，包括MMC,Cue,GameplayEffect,Ability,ASCPreset。
- 脚本生成路径: GAS的基础配置（Tag，Attribute，AttributeSet）都会有对应的脚本生成。

首次设置完路径后,点击检查子目录文件夹，确保必要的子文件夹都已生成。
>【生成AbilitySystemComponentExtension类脚本】这个按钮，请在生成了Attribute，AttributeSet，Ability的Lib集合类之后再点击。特别提醒，AbilitySystemComponentExtension是工具类，理论上只生成一次即可。


2. 配置Tag:  Tag是GAS核心逻辑运作的依赖,非常重要。关于Tag的使用及运作逻辑详见章节([GameplayTag](#22-gameplaytag))

3. 配置Attribute:  Attribute是GAS运行时数据单位。关于Attribute的使用及运作逻辑详见章节([Attribute](#23-attribute))

4. 配置AttributeSet:  AttributeSet是GAS运行时数据单位集合，合理的AttributeSet设计能够帮助程序解耦，提高开发效率。关于AttributeSet的使用及运作逻辑详见章节([AttributeSet](#24-attributeset))

5. 设计MMC,Cue:  详见[MMC](#25-modifiermagnitudecalculation), [GameplayCue](#26-gameplaycue)

6. 设计Gameplay Effect:  详见 [Gameplay Effect](#27-gameplayeffect)

7. 设计Ability:  详见 [Ability](#28-ability)

8. 设计ASC预设（可选）:  详见 [AbilitySystemComponent](#29-abilitysystemcomponent)


---
## 2.EX-GAS系统介绍
### 2.1 EX-GAS概述
>EX-GAS是对UnrealEngine的GAS（Gameplay Ability System）的模仿和实现。

GAS 是 "Gameplay Ability System" 的缩写，是一套游戏能力系统。
这个系统的目的是为开发者提供一种灵活而强大的框架，用于实现和管理游戏中的各种角色能力、技能和效果。

如果把EX-GAS高度概括为一句话，那就是：**WHO DO WHAT**。
- Who：AbilitySystemComponent（ASC）,EX-GAS的实例对象，是体系运转的基础单位
- Do：Ability，是游戏中可以触发的一切行为和技能
- What：GameplayEffect(GE)，掌握了游戏内元素的属性实际控制权，GameplayEffect本身应该理解为结果

GAS本质是一套属性数值的管理系统，GameplayCue我个人理解为附加价值（虽然这个附加价值很有分量）。
纵使GAS的Tag体系解决复杂的GameplayEffect和Ability的逻辑，但最终的结果目的也只是掌握属性数值变化。
而属性的最底层修改权力交由了GameplayEffect。所以我把GE理解为结果。

UE的GAS的使用门槛很高，这一点在我构筑完EX-GAS雏形后更是深有体会。
所以在EX-GAS的设计上，我尽可能的做简化，优化，来降低了使用门槛。
我制作了几个关键的编辑器，来帮助开发者快速的使用EX-GAS。
但即便如此，GAS本身的繁多参数依然让编辑器的界面看上去十分臃肿，这很难简化，没有哪个参数是可以被删除的。
甚至，雏形阶段的EX-GAS还有很多功能还未实现，也就是说还有更多的参数是没有被编辑器暴露出来的。

_**GAS的使用者必须至少有一名程序开发人员，因为GAS的使用需要编写大量自定义业务逻辑。
Ability，Cue，MMC等都是必须根据游戏类型和内容玩法而定的。
非程序开发人员则需要完全理解EX-GAS的运作逻辑，才能更好的配合程序开发人员快速配置出各种各样的技能，完善玩法表现。**_

### 2.2 GameplayTag
>Gameplay Tag,标签,它用于分类和描述对象的状态，非常有用于控制游戏逻辑。

- Gameplay Tag以树形层级结构（如Parent.Child.Grandchild）组织，用于描述角色状态/事件/属性/等，如眩晕（State.Debuff.Stun）。
- Gameplay Tag主要用于替代布尔值或枚举值，进行逻辑判断。
- 通常将Gameplay Tag添加到AbilitySystemComponent（ASC）以与系统（GameplayEffect，GameplayCue,Ability）交互。

Gameplay Tag在GAS中的使用涉及到标签的添加、移除以及对标签变化的响应。
开发者可以通过[GameplayTag Manager](#22a-gameplaytag-manager)在项目设置中管理这些标签，无需手动编辑配置文件。
Gameplay Tag的灵活性和高效性使其成为GAS中控制游戏逻辑的重要工具。
它不仅可以用于简单的状态描述，还可以用于复杂的游戏逻辑和事件触发。

>举个例子，GameplayEffect中有一个字段RequiredTags，其含义是当前GameplayEffect生效的AbilitySystemComponent（ASC）
需要拥有【所有】的RequiredTags（需求标签）。

上述例子，如果用传统的思路去做，可能需要写很多if-else判断，同时元素的实例脚本可能会增加很多状态标记的变量，
而且还需要考虑多个游戏效果的交互，这使得代码的设计和实现变得复杂，耦合。

GameplayTag的使用可以大大简化这些逻辑，使得代码更加清晰，易于维护。
他把状态和标记全部抽象成了一个独立的Tag系统，而且最巧妙的是树形结构的设计。
他解决了很多Gameplay设计上的问题，常见的问题比如：移除所有Debuff，传统的做法可能是让（中毒，减速，灼伤，等等）继承自Debuff类/接口；
而GameplayTag只需要添加一个Tag（中毒:Debuff.Poison，减速:Debuff.SpeedDown，灼伤:Debuff.Burning）

GameplayTag自身可以作为一个独立的系统去使用。
我在开发Demo的过程中就发现了GameplayTag的强大之处，他几乎替代了我的所有状态值。
甚至我设计了一个全局ASC，专门用来管理全局状态，我不需要对每个系统的状态管理，转而维护一个ASC即可。（虽然最后并没有落地这个设计，因为DEMO没有那么复杂。）

#### 2.2.a GameplayTag Manager
![QQ20240313114652.png](Wiki%2FQQ20240313114652.png)
我模仿了UE的GAS的Tag管理视图，做了树结构管理。通过选中节点，可以添加子节点，删除节点。要修改父子级关系时，只需要拖动节点即可。
Tag的分类设计需要谨慎，策划（设计师）需要再项目初就规划好Tag的大致分类。如果开发后期出现了Tag的父子级关系大变动，会严重影响原有游戏运作逻辑。

**【注意!!!】  每次编辑完Tag后，一定要点击右上角的【生成TagLib】按钮。GTagLib是包含了当前所有Tag的库类，
便于程序开发使用。GTagLib中Tag变量名会用‘_’代替‘.’ ，比如 State.Buff.PowerUp ->  State_Buff_PowerUp**

### 2.3 Attribute
>Attribute，属性，是GAS中的核心数据单位，用于描述角色的各种属性，如生命值，攻击力，防御力等。

Attribute和AttributeSet（属性集）需要结合起来才能作为唯一标识，简单点说AttributeSet是姓氏，Attribute是名字。
不同的AttributeSet可以有相同名字的Attribute，但是同一组的AttributeSet不可以有相同名字的Attribute。
常见的情况，如下：
> AttributeSet 人物: 生命值, 法力, 攻击力, 防御力
> 
> AttributeSet 武器: 生命值（耐久度）, 攻击力, 防御力
> 
> 而这两组AttributeSet中的生命值，攻击力，防御力，都是不同的属性，他们的意义和作用不同。但他们可以属于同一个单位。
#### 2.3.a Attribute Manager
![QQ20240313115953.png](Wiki%2FQQ20240313115953.png)
Attribute Manager的作用很简单，提供属性名字的管理。然后作为AttributeSet的选项使用。

**【注意!!!】  每次编辑完Attribute后，一定要点击【生成AttrLib】按钮。
只有AttrLib生成后，AttributeSet的Attribute选项才会发生改变。**

### 2.4 AttributeSet
>AttributeSet，属性集，是GAS中的核心数据单位集合，用于描述角色的某一类别的属性集合。

在上文的[2.3 Attribute]中，我们提到了AttributeSet是姓氏，Attribute是名字。二者结合起来才能作为唯一标识。
而对于AttributeSet的设计，可以较为随意，大多数情况，大家会更乐意一个单位只有一个AttributeSet。
因为这样便于管理和分类，不同类别的单位直接使用不同的AttributeSet。但实际上一个单位是可以拥有复数AttributeSet。
我其实比较认同一个单位只有一个AttributeSet的设计，因为这对程序开发也是好事，逻辑处理会更简单直白。

配置时的注意项：
- AttributeSet的名字禁止重复或空。这是因为AttributeSet的名字会作为类名。
- AttributeSet内的Attribute禁止重复。

#### 2.4.a AttributeSet Manager
![QQ20240313121300.png](Wiki%2FQQ20240313121300.png)
AttributeSet Manager统筹属性集的命名和属性管理。

**【注意!!!】  每次编辑完后，一定要点击【生成AttrSetLib】按钮。AttrSetLib会在AbilitySystemComponent的预设配置中用到。
AttrSetLib.gen.cs脚本中会包含所有的AttributeSet类（详见下文API章节），AttributeSet对应的类名是：“AS_名字”。
比如AttributeSet名字是Fight，那它对应的类名是AS_Fight。**

### 2.5 ModifierMagnitudeCalculation
>ModifierMagnitudeCalculation，修改器，负责GAS中Attribute的数值计算逻辑。

MMC(下文开始会使用缩写指代ModifierMagnitudeCalculation)唯一的使用场景是在GameplayEffect中。
GAS中，体系内运作的情况下，只有GameplayEffect才能修改Attribute的数值。而GameplayEffect就是通过MMC修改Attribute的数值。

MMC具有以下特点：
- 【与Attribute集成】： MMC 与 GAS 中的Attribute系统一起使用。这意味着计算效果幅度时可以考虑角色的属性，使得效果的强度与角色的属性值相关联。
- 【实时性】： MMC 用于在运行时动态计算 Attribute 的修改幅度。这样可以根据角色的状态、属性或其他因素，实时地调整效果的强度。
- 【自定义】： 通过继承 MMC的基类，开发者可以实现自定义的计算逻辑。这允许在计算效果幅度时考虑复杂的游戏逻辑、属性关系或其他条件。
- 【复用性】： 由于 MMC 是一个独立的类(Scriptable Object)，开发者可以在多个 GameplayEffect 中重复使用相同的计算逻辑。这样可以确保在整个游戏中保持一致的效果计算。
- 【灵活性】： 使用 MMC 提高了系统的灵活性，使得效果的强度不再是固定的数值，而可以根据需要在运行时进行调整，适应不同的游戏情境和需求。

MMC在GameplayEffect中的运作逻辑，结合GameplayEffect配置中MMC界面来解释。如下图：

![QQ20240313145154.png](Wiki%2FQQ20240313145154.png)

MMC被存储在Modifier中，Modifier是GameplayEffect的一部分。
Modifier包含了修改的属性，幅值（Magnitude），操作类型和MMC。
- 修改的属性：指的是GameplayEffect作用对象被修改的属性。可以看到属性名是“AS_Fight.POSTURE”，这对应了上文的提到的属性识别是AttrSet和Attr组合而成的。
- 幅值Magnitude：修改器的基础数值。这个数值如何使用由MMC的运行逻辑决定。
- 操作类型：是对属性的操作类型，有3种：
  - Add ： 加法（取值为负便是减法）
  - Multiply： 乘法（除法取倒数即可）
  - Override：覆写属性值
- MMC：计算单位，Modifier的核心，是一个ScriptableObject。MMC的类别如下：
  - ScalableFloatModCalculation：可缩放浮点数计算
    - 该类型是根据Magnitude计算Modifier模值的，计算公式为：`ModifierMagnitude * k + b`
      实际上就是一个线性函数，k和b为可编辑参数，可以在编辑器中设置。
  - AttributeBasedModCalculation：基于属性的计算
    - 该类型是根据属性值计算Modifier模值的，计算公式为：`AttributeValue * k + b`
      计算逻辑与ScalableFloatModCalculation一致。
    - 重点在于属性值的来源，确定属性值来源的参数有3个：
      - attributeFromType：属性值从谁身上取？是从游戏效果的来源（创建者），还是目标（拥有者）。
      - attributeName：属性值的名称，比如战斗属性集里的生命值：AS_Fight.Health
      - captureType：属性值的捕获类型
        - Track: 追踪,在Modifier被执行时，当场去取属性值
        - SnapShot: 快照,在游戏效果被创建时会对来源和目标的属性进行快照。在Modifier被执行时，去取快照的属性值。
  - SetByCallerModCalculation：由调用者设置的计算
    - 不使用任何值计算模值，而是在执行时由调用者给出Modifier模值。
    - 通过对GameplayEffectSpec注册数值来实现设置值。
    - 设置数值映射有2种：
      - 自定义键值：通过GameplayEffectSpec的RegisterValue(string key, float value)
      - GameplayTag：通过GameplayEffectSpec的RegisterValue(GameplayTag tag, float value)
  - CustomCalculation：自定义计算（必须继承自抽象基类ModifierMagnitudeCalculation）
    - 上述3种类型显然不够方便且全面的满足游戏开发者的所有需求，所以提供了自定义计算类的功能。
    - 允许开发者自由发挥给出各种各样的计算逻辑。

### 2.6 GameplayCue
>目前EX-GAS的GameplayCue功能还未完善。功能相对简陋。
- 【GameplayCue的作用】：GameplayCue是一个用于播放游戏提示的类，它的作用是在游戏运行时播放游戏效果，比如播放一个特效、播放一个音效等。
- 【GameplayCue的原则】： Cue是游戏提示，他必须遵守以下原则：
  - _**Cue不应该对游戏的数值体系产生影响，比如不应该对游戏的属性进行修改，不应该对游戏的Buff进行修改等。**_
  - _**Cue不应该对游戏玩法产生实际影响，比如即时战斗类的游戏，Cue不应该影响角色的位移、攻击等。**_

>第一条原则是所有类型游戏必须遵守的。
>
>而第二条原则就见仁见智了，因为游戏类型和玩法决定了cue的影响范围。
比如即时战斗类游戏，cue对角色位移有操作显然就是干涉了战斗，但如果是回合制游戏，cue对角色位移的操作就可以被当成是动画表现。
（甚至，即便是即时战斗类游戏，cue对角色位移的操作也可以被当成是动画表现，只要游戏开发人员认为cue的位移操作不影响游戏的战斗结果即可。）
- 【GameplayCue的类型】 GameplayCue的类型分两大类：
  - GameplayCueInstant：瞬时性的Cue，比如播放动画，伤害UI提示等
  - GameplayCueDurational：持续性的Cue，比如持续性的特效、持续性的音效等
>GameplayCueInstant和GameplayCueDurational都是抽象类，它们的子类才是真正的可使用Cue类。
Cue是需要程序开发人员大量实现的，毕竟游戏不同导致游戏提示千变万化。

- 【关于Cue的子类实现】： Cue的完整组成为GameplayCue和GameplayCueSpec：
  - GameplayCue< T >（抽象基类,T为对应的Spec类）：Cue的数据实类，是一个可编辑类，开发人员可以在编辑器中设置Cue的各种参数。该类只可以被视作数据类。
    - 必须实现CreateSpec方法：用于创建对应的Spec类
  - GameplayCueSpec（抽象基类）：Cue的规格类，是Runtime下Cue的真正实例，Cue的具体逻辑在该类中实现。
    - | Spec类|需要实现的方法| 方法触发时机   |
      |---|---|----------|
      |GameplayCueInstantSpec<br/>瞬时性Cue的规格类|Trigger()| 执行时触发|
      |GameplayCueDurationalSpec<br/>持续性Cue的规格类|OnAdd()| Cue被添加时触发|
      |GameplayCueDurationalSpec|OnRemove()| Cue被移除时触发|
      |GameplayCueDurationalSpec|OnGameplayEffectActivated()| Cue所属的GameplayEffect被激活时触发|
      |GameplayCueDurationalSpec|OnGameplayEffectDeactivated()| Cue所属的GameplayEffect被移除时触发|
      |GameplayCueDurationalSpec|OnTick()| Cue的每帧更新逻辑|

- 【关于Cue的参数传递】： 目前EX-GAS的Cue参数传递非常简陋，依赖于结构体GameplayCueParameters，成员如下：
  - GameplayEffectSpec sourceGameplayEffectSpec：Cue所属的GameplayEffect实例（如果是GE触发）
  - AbilitySpec sourceAbilitySpec：Cue所属的Ability实例（如果是Ability触发）
  - object[] customArguments：自定义参数，不同于GameplayCue中的数据。
    customArguments是供程序开发人员在业务逻辑内自由传递参数的载体。
>注意：customArguments是一个object数组，开发人员需要自己保证传递的参数类型正确，否则会导致运行时错误。
customArguments是最暴力的设计，往后EX-GAS的Cue参数传递设计还会进行优化。
- 【GameplayCue的使用】：
GameplayCue的使用手段很多，最基础的是在GameplayEffect中使用，Cue最开始的设计基础也是依附于GameplayEffect。Ability也可以对Cue进行操作。
除此之外，Cue的使用不限制于EX-GAS的体系内。开发者可以在任何地方使用Cue，只要能获取到GameplayCue的资源实例并且遵守Cue的原则即可。
  - 在GameplayEffect中使用Cue 
    - GameplayEffect中使用Cue会根据GameplayEffect执行策略产生变化。
      - | GameplayEffect类型 |Cue名称|Cue类别| 执行时机                    |
        |------------------|---|---|-------------------------|
        | Instant          |CueOnExecute|Instant| GameplayEffect执行时触发     |
        | Durational       |CueDurational|Durational| 生命周期完全和GameplayEffect同步 |
        | Durational       |CueOnAdd|Instant| GameplayEffect添加时触发     |
        | Durational       |CueOnRemove|Instant| GameplayEffect移除时触发     |
        | Durational       |CueOnActivate|Instant| GameplayEffect激活时触发     |
        | Durational       |CueOnDeactivate|Instant| GameplayEffect失活时触发     |

  - 在Ability中使用Cue
    - Ability种Cue的使用完全依赖于Ability自身的业务逻辑，因此程序开发者在AbilitySpec中实现Cue逻辑时一定要保证合理性。
      特别是对于Durational类型的Cue，一定要保证Cue生命周期的合理性，切记不要出现遗漏销毁Cue的情况。
    
### 2.7 GameplayEffect
>GameplayEffect是EX-GAS的核心之一，一切的游戏数值体系交互基于GameplayEffect。

GameplayEffect掌握了游戏内元素的属性控制权。理论上，只有它可以对游戏内元素的属性进行修改
（这里指的是修改，数值的初始化不算是修改）。当然，实际情况下，游戏开发人员当然可以手动直接修改属性值。
但是还是希望游戏开发者尽可能的不要打破EX-GAS的数值体系逻辑，因为过多的额外操作可能会导致游戏的数值体系变得混乱，难以追踪数值变化等等。

另外GameplayEffect还可以触发Cue（游戏提示）完成游戏效果的表现，以及控制获取额外的能力等。

- GameplayEffect的使用
![QQ20240313152015.png](Wiki%2FQQ20240313152015.png)

GameplayEffect的配置界面如图，接下来逐一解释各个参数的含义。
  - Name
    - GameplayEffect的名称，纯粹用于显示，不会影响游戏逻辑。方便编辑者区分GameplayEffect。
  - Description
    - GameplayEffect的描述，纯粹用于显示，不会影响游戏逻辑。方便编辑者阅读理解GameplayEffect。
  - DurationPolicy：GameplayEffect的执行策略，有以下几种：
    - | 策略类型 | 执行逻辑                                                                                                | 
        |---|-----------------------------------------------------------------------------------------------------|
        | Instant | 即时执行，GameplayEffect被添加时立即执行，执行完毕后销毁自身。                                                              |
        | Duration | 持续执行，GameplayEffect被添加时立即执行，持续时间结束后移除自身。                                                            |
        | Infinite | 无限执行，GameplayEffect被添加时立即执行，执行完毕后不会移除，需要手动移除。                                                       |
  - Duration
    - 持续时间，只有DurationPolicy为Duration时有效。
  - Every(Period)
    - 周期，只有DurationPolicy为Duration或者Infinite时有效。每隔Period时间执行一次PeriodExecution。
  - PeriodExecution
    - 周期执行的GameplayEffect，只有DurationPolicy为Duration或者Infinite，且Period>0时有效。每隔Period时间执行一次PeriodExecution。
      _**PeriodExecution禁止为空!!**_PeriodExecution原则上只允许是Instant类型的GameplayEffect。但如果根据开发者需求，也可以使用其他类型的GameplayEffect。
  - GrantedAbilities(**该功能目前尚未生效**)
    - 授予的能力，只有DurationPolicy为Duration或者Infinite时有效。在GameplayEffect生命周期内，GameplayEffect的持有者会被授予这些能力。
        GameplayEffect被移除时，这些能力也会被移除。
  - Modifiers: 属性修改器。详见[MMC](#25-modifiermagnitudecalculation)
- Tags：标签。Tag具有非常重要的作用，合适的tag可以处理GameplayEffect之间复杂的关系。
  - | Tag类型 | 作用                                                                                   |
    |---|----------------------------------------------------------------------------------------|
    | AssetTags | 描述性质的标签，用来描述GameplayEffect的特性表现，比如伤害、治疗、控制等。 |
    | GrantedTags | GameplayEffect的持有者会获得这些标签，GameplayEffect被移除时，这些标签也会被移除。Instant类型的GameplayEffect的GrantedTags是无效的。 |
    | ApplicationRequiredTags | GameplayEffect的目标单位必须拥有【所有】这些标签，否则GameplayEffect无法被施加到目标身上。 |
    | OngoingRequiredTags | GameplayEffect的目标单位必须拥有【所有】这些标签，否则GameplayEffect不会被激活。 （施加和激活是两个概念，如果已经被施加的GameplayEffect持续过程中，目标的tag变化了，不满足，效果就会失活；满足了，就会被激活）。Instant类型的GameplayEffect的OngoingRequiredTags是无效的。|
    | RemoveGameplayEffectsWithTags | GameplayEffect的目标单位当前持有的所有GameplayEffect中，拥有【任意】这些标签的GameplayEffect会被移除。 |
    | Application Immunity Tags | GameplayEffect的目标单位拥有【任意】这些标签，就对该GameplayEffect免疫。 |
  - DurationPolicy为Instant时
    - CueOnExecute（Instant）：GameplayEffect执行时触发。
  - DurationPolicy为Duration或者Infinite时
    - CueDurational（Durational）：生命周期完全和GameplayEffect同步
    - CueOnAdd（Instant）：GameplayEffect添加时触发
    - CueOnRemove（Instant）：GameplayEffect移除时触发
    - CueOnActivate（Instant）：GameplayEffect激活时触发。
    - CueOnDeactivate（Instant）：GameplayEffect失活时触发。
> GameplayEffect的施加（Apply）和激活（Activate）
>   - GameplayEffect的施加（Apply）和激活（Activate）是两个概念，施加是指GameplayEffect被添加到目标身上，激活是指GameplayEffect实际生效。
>      - 为什么做区分？
>      - 举个例子：固有被动技能（Ability）是持续回血，被动技能的逻辑显然是永久激活的状态，而持续回血的效果（GameplayEffect）
>        来源于被动技能，那如果单位受到了外部的debuff禁止所有的回血效果，那么是不是被动技能被禁止？显然不是，被动技能还是会持续激活的。
>        那应该是移除回血效果吗？显然也不是，被动技能整个过程是不做任何变化，如果移除回血效果，那debuff一旦消失，谁再把回血效果加回来？
>        所以，这里需要区分施加和激活，被动技能的持续回血效果被施加到单位身上，而debuff做的是让回血效果失活，而不是移除回血效果，一旦debuff结束，
>        回血效果又被激活，而这个激活的操作可以理解为回血效果自己激活的（依赖于Tag系统）。

### 2.8 Ability
> Ability是EX-GAS的核心类之一，它是游戏中的所有能力基础。
>
> 同时Ability也是程序开发人员最常接触的类，Ability的完整逻辑都是由程序开发人员实现的。

在EX-GAS内，Ability是游戏中可以触发的一切行为和技能。多个Ability可以在同一时刻激活, 例如移动和持盾防御。
Ability作为EX-GAS的核心类之一，他起到了Do（做）的功能。

Ability的业务逻辑取决于游戏类型和玩法。所以不存在一个通用的Ability模板，当然可以针对游戏类型制作一些通用的ability。
Ability的逻辑并非自由，如果胡乱的实现Ability逻辑，可能会导致游戏逻辑混乱，所以需要遵循一些规则。

Ability的具体实现需要策划和程序配合。
这并不是废话，而是在EX-GAS的Ability制作流程中，确确实实的把策划和程序的工作分开了：
- 策划的工作：配置AbilityAsset
- 程序的工作：编写Ability（AbilityAsset,Ability,AbilitySpec）类
- | 类                        | 功能                                                                                |
  |--------------------------|-----------------------------------------------------------------------------------|
  | AbilityAsset  | Ability的配置文件，同一类的Ability可以通过配置不同的AbilityAsset参数，实现复数Ability，比如跳跃段数不同，可以实现普通跳，二段跳。 |
  | Ability  | Ability的Runtime数据类，通常数据还是依赖AbilityAsset。同时也允许运行时不依赖AbilityAsset生成Ability          |
  | AbilitySpec   | Ability的运行实例，Ability的游戏内的表现逻辑，就在该类中实现。                                            |
>建议 AbilitySpec和Ability在同一个脚本中编辑，因为二者本身就是成对出现。AbilityAsset单独一个脚本，因为它是Scriptable Object，应该遵从脚本名和类一致的原则。

Ability运作逻辑的组成可以拆成两部分：
- GAS系统内的运作逻辑：所有Ability通用的数据字段，被保存在AbstractAbility这个抽象基类中。所有的Ability都是继承自AbstractAbility。
- 具体游戏内的表现逻辑：每个Ability都有自己的表现逻辑，这部分逻辑是由程序开发人员自行实现的。

接下来结合Ability的配置界面来解释Ability的数据和运作逻辑。
#### 2.8.a Ability编辑界面
![QQ20240313162642.png](Wiki%2FQQ20240313162642.png)
>图中A的部分就是GAS系统内的运作逻辑的参数，B的部分就是具体游戏内的表现逻辑的参数。

注意到最上方显示了Ability Class，这是Ability的类名，与之成对的AbilitySpec决定了Ability游戏内的表现逻辑。
- GAS系统内的运作逻辑参数
  - U-Name: Ability的名称，唯一标识符，禁止重复或空。U-Name会用于AbilityLib的Ability生成和查找。
  - Description: Ability的描述，纯粹用于显示，不会影响游戏逻辑。方便编辑者区分Ability。
  - 消耗Cost：Ability的消耗GameplayEffect
  - 冷却CD：Ability的冷却GameplayEffect
  - CD时间：冷却时间，它会覆盖冷却GameplayEffect的持续时间。
  - Tags: Ability的标签,与GameplayEffect的Tag些许类似。具体Tag功能如下

| Tag                      | 功能                                                                |
|--------------------------|-------------------------------------------------------------------|
| Asset Tag                | 描述性质的标签，用来描述Ability的特性表现，比如伤害、治疗、控制等                              |
| CancelAbility With Tags  | Ability激活时，Ability持有者当前持有的所有Ability中，拥有**【任意】**这些标签的Ability会被取消   |
| BlockAbility With Tags       | Ability激活时，Ability持有者当前持有的所有Ability中，拥有**【任意】**这些标签的Ability会被阻塞激活 |
| Activation Owned Tags    | Ability激活时，持有者会获得这些标签，Ability被失活时，这些标签也会被移除                       |
| Activation Required Tags | Ability只有在其拥有者拥有**【所有】**这些标签时才可激活                                 |
| Activation Blocked Tags  | Ability在其拥有者拥有**【任意】**这些标签时不能被激活                                       |
   
- 具体游戏内的表现逻辑参数
  - 这部分的参数面板是由程序开发人员自行实现的，这部分参数的含义和作用不固定。
  - 建议使用Odin的特性编辑，一是为了规范，二是美观。 
  - 不同类的Ability，这部分的面板显示和含义不同。如下图：
  ![QQ20240313165819.png](Wiki%2FQQ20240313165819.png)


- **【！！注意！！】**
  - **当创建或修改AbilityAsset的U-Name后，一定要去Ability汇总界面点击【生成AbilityLib】按钮。
  AbilityLib.gen.cs脚本中会包含所有的Ability信息。
  游戏运行时生成Ability依赖于AbilityLib，如果没有保持同步，会导致游戏运行时找不到Ability。**
  - Ability汇总界面入口：在菜单栏EX-GAS -> Asset Aggregator -> 左侧菜单列点击 C-Ability
![QQ20240313175247.png](Wiki%2FQQ20240313175247.png)
  
#### 2.8.b TimelineAbility 通用性Ability（W.I.P TimelineAbility还在完善中）
在实际的开发过程中，我发现，许多的Ability都有顺序和时限两个特点。
每次都新写一个Ability类来实现某个指定技能让我十分烦躁，于是我制作了TimelineAbility，一个极具通用性的顺序，时限Ability。
![TimelineAbilityAsset.png](Wiki%2FTimelineAbilityAsset.png)
>这是TimelineAbilityAsset的面板。

TimelineAbilityAsset的大多数表现逻辑参数在AbilityAsset面板都是隐藏的（HideInInspector）。
转而都是在TimelineAbilityEditor面板中可视化编辑。
唯一在AbilityAsset面板中可见的参数是【手动结束能力】的bool值选项。这个选项决定Ability是手动结束还是播放完成后自动结束。

通过点击【查看/编辑能力时间轴】按钮，可以打开TimelineAbilityEditor面板。
![TimelineAbilityEditor.png](Wiki%2FTimelineAbilityEditor.png)
>这是TimelineAbilityEditor的面板。

接下来详细介绍TimelineAbilityEditor的面板参数含义及操作逻辑。
- 顶部菜单栏
    ![QQ20240315133711.png](Wiki%2FQQ20240315133711.png)
    - Ability配置 ： 当前编辑的TimelineAbilityAsset
    - 查看能力基本信息：点击按钮后，右侧的子Inspector会显示TimelineAbilityAsset的面板信息，和Asset Aggregator中的AbilityAsset面板一致。
    - 预览实例：场景内的GameObject. 选取后，TimelineAbility会以该GameObject为参照物，预览TimelineAbility的表现逻辑。
      用到的常见预览有Animation，特效，物体挂载，伤害碰撞盒等等。
    - 预览场景：点击该按钮后会，进入空场景。目前还没有自定义LookDev场景，所以只是一个临时创建空场景。
    - 返回旧场景：点击该按钮后会，返回到原来的场景。
    - 显示子Inspector：点击该按钮后会，刷新显示右侧的子Inspector的布局。
- 时间轴编辑部分
  - 左侧播放栏
    ![QQ20240315143604.png](Wiki%2FQQ20240315143604.png)
    - 【<】:上一帧，只有当预览实例不为空时，才会生效。
    - 【>】:下一帧，只有当预览实例不为空时，才会生效。
    - 【▶】：播放/暂停 ，只有当预览实例不为空时，才会生效。
    - 左侧帧数：当前预览帧数，只有当预览实例不为空时，才会生效。
    - 右侧帧数：Ability执行总帧数
  - 左侧轨道菜单栏
    - ![QQ20240315144012.png](Wiki%2FQQ20240315144012.png)
    - TimelineAbility基础轨道有6种。【添加轨道只需点击右侧的‘+’，删除轨道只需右键对应轨道选择Delete Track即可】
      1. Instant Cue 【即时Cue轨道】
         - 轨道Item类型：Mark ![QQ20240315151141.png](Wiki%2FQQ20240315151141.png)
         - 一个Mark可以挂多个Cue。理论上，一个TimelineAbility只需要一条Instant Cue轨道。
         ![QQ20240315152528.png](Wiki%2FQQ20240315152528.png)
         - 扩展：详见上文中提到的Instant Cue自定义实现
      2. Release Effect【GameplayEffect释放轨道】
         - 轨道Item类型：Mark
         - 一个Mark持有一个TargetCatcher和数个GameplayEffectAsset
           ![QQ20240315153247.png](Wiki%2FQQ20240315153247.png)
           -  TargetCatcher：GameplayEffect释放需要对象，而TargetCatcher的作用就是找到这些对象。
              TargetCatcher固有初始化会获取Owner（ASC），核心是方法CatchTargets()。 基类如下：
           ```
             public abstract class TargetCatcherBase
             {
                public AbilitySystemComponent Owner;
                public TargetCatcherBase()
                {
                }
                public virtual void Init(AbilitySystemComponent owner)
                {
                    Owner = owner;
                }
                // mainTarget为TimelineAbility的指向性目标单位，为可选参数。具体在API中会介绍。
                public abstract List<AbilitySystemComponent> CatchTargets(AbilitySystemComponent mainTarget);
             }
           ```
             我提供了几个基础TargetCatcher（后续会陆续添加常用的Catcher）
         
           - 
            | Catcher名          | 作用                 |
            |-------------------|--------------------|
            | CatchSelf         | 捕捉自己               |
            | CatchTarget       | 捕捉指向性目标            |
            | CatchAreaBox2D    | 捕捉2d矩形内的目标（适用2D游戏） |
            | CatchAreaCircle2D | 捕捉2d圆形内的目标（适用2D游戏） |
           - TargetCatcher的UI面板绘制：
            自定义TargetCatcher的UI面板需要继承自TargetCatcherInspector<T> T为TargetCatcher类
            **（必须直接继承，不可以多级继承，因为Inspector的Type查找规则依赖第一泛型类做匹配）**
         - Release Effect的执行逻辑：先调用TargetCatcher的CatchTargets()方法，然后对捕获的目标单位施加所有指定GameplayEffect。
      3. Instant Task 【即时Task轨道】
         - 轨道Item类型：Mark
         - 一个Mark可以挂载复数的Instant Task。 关于Ability Task的详细介绍见[下文](#)。
           ![QQ20240315170234.png](Wiki%2FQQ20240315170234.png)
         - Task是自定义事件，可以是任何游戏逻辑，纯粹由开发者决定。
         - Instant Task的面板绘制：自定义Task的UI面板需要继承自InstantTaskInspector<T> T为InstantAbilityTask类
           **（必须直接继承，不可以多级继承，因为Inspector的Type查找规则依赖第一泛型类做匹配）**
      4. Durational Cue【持续GameplayCue轨道】
         - 轨道Item类型：Clip ![QQ20240315164448.png](Wiki%2FQQ20240315164448.png)
         - 每段Clip只含一个Duration Cue ![QQ20240315164632.png](Wiki%2FQQ20240315164632.png)
         - **注意！！ TimelineAbility下的持续性Cue，
           只会执行OnAdd（Cue播放的第一帧），OnRemove（Cue播放的最后一帧），OnTick，和GameplayEffect相关的方法不会被执行**
      5. Buff【Buff轨道】
         - 轨道Item类型：Clip
         - 每段Clip只含一个Buff（GameplayEffect），且Buff的作用对象只会是Ability的持有者自己。
           ![QQ20240315165106.png](Wiki%2FQQ20240315165106.png)
         - **【注意！！】请确保设置的GameplayEffect类型为Durational或Infinite。
            非持续类型的GameplayEffect不会生效。且GameplayEffect执行时会设置为Infinite执行策略，
            生命周期由Clip长度（Duration）决定。**
      6. Ongoing Task【持续Task轨道】
         - 轨道Item类型：Clip
         - 每段Clip只含一个Ongoing Task。 关于Ability Task的详细介绍见[下文](#)。
           ![QQ20240315170648.png](Wiki%2FQQ20240315170648.png)
         - Ongoing Task的面板绘制：自定义Task的UI面板需要继承自OngoingTaskInspector<T> T为OngoingAbilityTask类
           **（必须直接继承，不可以多级继承，因为Inspector的Type查找规则依赖第一泛型类做匹配）**
      
  - 右侧Inspector
    - 所有Track，TrackItem参数在点击后都会显示在Inspector上

TimelineAbility的执行逻辑很直观，就是沿着时间轴从左往右执行，每个轨道的Item都会在对应的时间点执行。
所有事件的执行，都遵照时间轴的顺序，以及Mark内参数排列顺序。如果存在前后逻辑关系，那么配置的时候请务必注意顺序。
最大帧数决定了Ability的执行时间，如果【手动结束能力】设置为false，那么在播放完TimelineAbility后，会自动调用TryEndAbility()，
反之，需要开发人员在代码中决定调用TryEndAbility()的时机。

TimelineAbility的配置可能还满足不了一些设计时，程序开发人员可以对TimelineAbility进行继承，拓展功能需求。
> 特别是在TargetCatcher，AbilityTask的自定义上，还是很可能遇到这个问题。
> 因为TargetCatcher和AbilityTask的持续化存储是以JsonData的格式，ScriptableObject类型参数的Json存储是存在GUID不匹配问题的。
> 所以，TargetCatcher和AbilityTask的参数中，不建议出现ScriptableObject类型的参数。

---
### 2.9 AbilitySystemComponent
> AbilitySystemComponent是EX-GAS的核心之一，它是GAS的基本运行单位。

ASC(之后都使用缩写指代AbilitySystemComponent),持有Tag，Ability，AttributeSet，GameplayEffect等数据。
其主要职责如下：
- 管理能力（Abilities）： ASC 负责管理角色的所有能力。它允许角色获得、激活、取消和执行各种不同类型的能力，如攻击、防御、技能等。
- 处理效果（GameplayEffects）： ASC 负责处理与能力相关的效果，包括伤害、治疗、状态效果等。它能够跟踪和应用这些效果，并在需要时触发相应的回调或事件。
- 处理标签（Tags）： ASC 负责管理角色身上的标签。标签用于标识角色的状态、属性或其他特征，以便在能力和效果中进行条件检查和过滤。
- 处理属性（Attributes）： ASC 负责管理角色的属性。属性通常表示角色的状态，如生命值、能量值等。ASC 能够增减、修改和监听这些属性的变化。

整个GAS的运作都是围绕着ASC的，所有的Tag，GameplayEffect的作用对象最后都是ASC。而Ability也必须依赖ASC来执行。
为了直观的理解ASC，接下来参考GAS Watcher的监视器界面：
![QQ20240313180923.png](Wiki%2FQQ20240313180923.png)

- ID Mark:这部分可以忽略，只是Unity运行时的Gameobject的标识符相关
- Ability： 图中显示了这个单位有6个Ability。Ability表示了单位的持有能力，但不代表所有的单位能力都要由Ability来做。
    虽然我把Move作为Ability了，但是在FPS,MOBA等类型的游戏中，像Move移动这种强Input关联的行为是不建议用Ability来做的。
- Attribute：单位持有的属性。
  ASC的属性是以AttributeSet为集合，通常ASC的属性集，只加不减。
  因为删除属性集存在很大的隐患，在GAS体系运作时，属性相当于串联GAS运行的线，线上挂着许多GameplayEffect，贸然删除属性集可能会导致线断裂，GameplayEffect运行逻辑失效甚至崩溃。
- GameplayEffects：单位当前持有的GameplayEffect。这里可以直接将GameplayEffect理解为Buff。
  GAS体系内，ASC之间是通过GameplayEffect来交互。
   比如A单位对B单位施法，那么A单位会通过施法（Ability）对B施加一个效果（GameplayEffect），效果如果是持续性的就会被留在B身上（Buff）。
    当然，GAS体系外时，可以通过接口对ASC单位添加/移除GameplayEffect。通常这种情况下，Effect的来源和目标都是指定的那个ASC。
- Tags：单位当前持有的Tag。标签的价值只有在被挂载到ASC时才会体现出来。所有Tag的比较逻辑都直接或间接依赖于ASC。
  - Fixed Tag：固有标签，这些标签是ASC固有的。GAS体系内是不会对FixedTag做任何增减的。
    只有GAS体系外的才会对FixedTag有影响。FixedTag通常是在ASC初始化的时候设置好，之后就尽可能不动。
  - Dynamic Tag：动态标签，这些标签是ASC动态增减的。GAS体系只会对DynamicTag做增减，且只有GAS体系可以管理。
    GAS体系外只能通过GameplayEffect的一些指定接口，间接对DynamicTag进行操作。

ASC是GAS中最复杂，且操作空间最多的组件。对ASC的良好管理和操作就是程序开发人员的重任了。
GAS本身是被动的，而让推动和改变GAS的是ASC。换言之，Runtime下开发者其实是在操作ASC，而不是GAS。
增删管理ASC，调用ASC的Ability执行，以及ASC的体系外Tag，Effect管理才是Runtime下开发者的主要工作。
这之外的GAS配置和拓展，应该由策划承担大部分工作。（但实际上对于中小型团队，程序开发人员还是在做GAS配置的维护工作。）

#### 2.9.a AbilitySystemComponent Preset
AbilitySystemComponent Preset是ASC的预设，用于方便初始化ASC的数据。
![QQ20240315172608.png](Wiki%2FQQ20240315172608.png)
ASC预设是为了可视化角色（单位）的参数。
- 基本信息：ASC的基本信息，仅用于显示，方便配置人员阅读，Runtime不会用到这些参数。
- 属性集：上文提到过，ASC的属性集设计建议只有一个属性集。不建议多个。
- 固有Tag：ASC的基础Tag，通常会把描述性的Tag作为固有Tag，
  比如种族（Race.Human,Race.Monster ）,职业（Job.Wizard,Job.Archer）,阵营（Camp.Good,Camp.Evil）等等。
  当然Tag本身是不做任何限制的，但从Gameplay设计的角度上，状态性质的Tag不建议作为固有Tag。就算设计一个绝对无敌的
  角色，那也应该是把无敌的Tag放在一个永久GameplayEffect上，然后挂到ASC上。而不是把无敌Tag直接当作固有Tag。
- 固有能力：Abilities，单位的基础能力。通常会把单位的基础能力放在这里，比如攻击，防御，跳跃等等。

>如何使用ASC预设？
> 
> 1.AbilitySystemComponent组件自带了序列化的ASC预设字段，可以通过预制体添加，也可以实例化添加。
> 2.依赖ASC预设的初始化，通过AbilitySystemComponentExtension中的静态扩展方法InitWithPreset即可。
> 
> InitWithPreset的参数：
>  - AbilitySystemComponent asc：初始化的ASC，
>  - int level：初始等级
>  - AbilitySystemComponentPreset preset：初始化用的ASC预设
>  - 
> 示例： asc.InitWithPreset(1,ascPreset); // 如果预制体已经设置了参数，那么可以不传ascPreset。

---
## 3.API && Source Code Documentation
本章节会在介绍API和源码的同时，从代码的角度来理解GAS的运作逻辑。
![GAS_IMG_Intro.png](Wiki%2FGAS_IMG_Intro.png)
> 该图简单的解释了GAS的运作逻辑。GAS其实简单的只干一件事：ASC使用Ability对指定的（可以包括自己）ASC释放GameplayEffect。
>
> GAS的推进和运行，就是在不断的重复这件事。
> 体系外的脚本不断的拨动ASC的Ability，而GAS内部会对Ability的运行结果自行消化。

### 3.1 Core 

#### 3.1.1 GameplayAbilitySystem
GameplayAbilitySystem作为核心类，他的作用有2个：管理ASC，控制GAS的运行与否。
- ` static GameplayAbilitySystem GAS`
  - GAS的单例，所有的GAS操作都是通过GAS单例来进行的。
- ` List<AbilitySystemComponent> AbilitySystemComponents { get; } `
  - GAS当前运行的所有AbilitySystemComponent的集合。
- `void Register(AbilitySystemComponent abilitySystemComponent)`
  -  注册ASC到GAS中。
- `void UnRegister(AbilitySystemComponent abilitySystemComponent)`
  -  从GAS中注销ASC。
- `bool IsPaused`
  - GAS是否暂停运行。
- ` void Pause()`
  - 暂停GAS运行。 
- ` void Unpause()`
  - 恢复GAS运行。 
#### 3.1.2 GASTimer
GASTimer是GAS的计时器，它是GAS的时间基准。
- `static long Timestamp()`
  - GAS当前时间戳（毫秒）
- `static long TimestampSeconds()`
  - GAS当前时间戳（秒）
- `static int CurrentFrameCount`
  - GAS当前运行帧数
- `static long StartTimestamp`
  - GAS启动时间戳
- `static void InitStartTimestamp()`
  - GAS初始化启动时间戳
- `static void Pause()`
  - 暂停GASTimer计时 
- `static void Unpause()`
  - 恢复GASTimer计时 
- `static int FrameRate`
  - GAS帧率
#### 3.1.3 GasHost
GasHost是GAS的宿主，它是GAS的运行机器和环境，GasHost没有API可以从外部干涉。

### 3.2 AbilitySystemComponent
#### 3.2.1 AbilitySystemComponent
AbilitySystemComponent是GAS的基本运行单位，它是GAS的核心类。
ASC的public方法和属性就是外部干涉GAS的唯一手段。
- `AbilitySystemComponentPreset Preset`
  - ASC的预设。外部读取用，修改preset需要通过SetPreset方法
- `void SetPreset(AbilitySystemComponentPreset preset)`
  - 修改ASC的预设。 
- `int Level { get; protected set; }`
  - ASC的等级
- `GameplayEffectContainer GameplayEffectContainer { get; private set; } `
  - ASC当前所有GameplayEffect的容器，可以通过GameplayEffectContainer对GameplayEffect进行一定的外部干涉。
- `GameplayTagAggregator GameplayTagAggregator { get; private set;} `
  - ASC的GameplayTag聚合器，单位的Tag全部都由聚合器管理，外部可以通过聚合器对Tag进行一定的外部干涉。
- `AbilityContainer AbilityContainer { get; private set;}`
  - ASC的Ability容器，可以通过AbilityContainer对Ability进行一定的外部干涉。 
- `AttributeSetContainer AttributeSetContainer { get; private set;}`
  - ASC的AttributeSet容器，可以通过AttributeSetContainer对AttributeSet进行一定的外部干涉。
- `void Init(GameplayTag[] baseTags, Type[] attrSetTypes,AbilityAsset[] baseAbilities,int level)`
  - 初始化ASC
  - baseTags：ASC的基础Tag
  - attrSetTypes：ASC的初始化AttributeSet类型
  - baseAbilities：ASC的初始化Ability
  - level：ASC的初始化等级
- `void SetLevel(int level)`
  - 设置ASC的等级
- `bool HasTag(GameplayTag gameplayTag)`
  - 判断ASC是否持有指定Tag
  - gameplayTag：指定Tag
  - 返回值：是否持有
- `bool HasAllTags(GameplayTagSet tags)`
  - 判断ASC是否持有指定Tag集合中的所有Tag
  - tags：指定Tag集合
  - 返回值：是否持有
- `bool HasAnyTags(GameplayTagSet tags)`
  - 判断ASC是否持有指定Tag集合中的任意一个Tag
  - tags：指定Tag集合
  - 返回值：是否持有
- `void AddFixedTags(GameplayTagSet tags)`
  - 添加固有Tag
  - tags：添加的Tag集合
- `void RemoveFixedTags(GameplayTagSet tags)`
  - 移除固有Tag
  - tags：移除的Tag集合
- `void AddFixedTag(GameplayTag tag)`
  - 添加固有Tag
  - tag：添加的Tag
-  `void RemoveFixedTag(GameplayTag tag)`
  - 移除固有Tag
  - tag：移除的Tag
- `void RemoveGameplayEffect(GameplayEffectSpec spec)`
  - 移除指定的GameplayEffect
  - spec：指定的GameplayEffect的规格类实例
- `GameplayEffectSpec ApplyGameplayEffectTo(GameplayEffect gameplayEffect, AbilitySystemComponent target)`
  - 对指定的ASC施加指定的GameplayEffect
  - gameplayEffect：指定的GameplayEffect
  - target：目标ASC
  - 返回值：施加的GameplayEffect的规格类实例
- `GameplayEffectSpec ApplyGameplayEffectToSelf(GameplayEffect gameplayEffect)`
  - 对自己施加指定的GameplayEffect
  - gameplayEffect：指定的GameplayEffect
  - 返回值：施加的GameplayEffect的规格类实例
- `void GrantAbility(AbstractAbility ability)`
  - 获得指定的Ability
  - ability：指定的Ability
- `void RemoveAbility(string abilityName)`
  - 移除指定的Ability
  - abilityName：指定的Ability的U-Name
- `float? GetAttributeCurrentValue(string setName, string attributeShortName)`
  - 获取指定Attribute的当前值 
  - setName：AttributeSet的名字
  - attributeShortName：Attribute的短名
  - 返回值：Attribute的当前值
- `float? GetAttributeBaseValue(string setName, string attributeShortName)`
  - 获取指定Attribute的基础值 
  - setName：AttributeSet的名字
  - attributeShortName：Attribute的短名
  - 返回值：Attribute的基础值
- `Dictionary<string, float> DataSnapshot()`
  - 获取ASC的数据快照
  - 返回值：ASC的数据快照
- ` bool TryActivateAbility(string abilityName, params object[] args)`
  - 尝试激活指定的Ability
  - abilityName：指定的Ability的U-Name
  - args：激活Ability的参数
  - 返回值：是否激活成功
- `void TryEndAbility(string abilityName)`
  - 尝试结束指定的Ability
  - abilityName：指定的Ability的U-Name
- `void TryCancelAbility(string abilityName)`
  - 尝试取消指定的Ability
  - abilityName：指定的Ability的U-Name
- `void ApplyModFromInstantGameplayEffect(GameplayEffectSpec spec)`
  - 从Instant GameplayEffect中应用Mod
  - spec：Instant GameplayEffect的规格类实例
- `CooldownTimer CheckCooldownFromTags(GameplayTagSet tags)`
  - 通过Tag检查冷却时间
  - tags：指定的Tag集合
  - 返回值：冷却计时器
- `T AttrSet<T>() where T : AttributeSet`
  - 获取指定类的AttributeSet
  - 返回值：指定类的AttributeSet
- `void ClearGameplayEffect()`
  - 清空ASC的所有GameplayEffect
 
#### 3.2.2 AbilitySystemComponentPreset
AbilitySystemComponentPreset是ASC的预设，用于方便初始化ASC的数据。
- `string[] AttributeSets`
  - ASC的初始化AttributeSet类型 
- `GameplayTag[] BaseTags` 
  - ASC的基础Tag
- `AbilityAsset[] BaseAbilities`
  - ASC的初始化Ability
  
#### 3.2.3 AbilitySystemComponentExtension
AbilitySystemComponentExtension是ASC的扩展方法类，用于方便ASC的初始化和操作。
AbilitySystemComponentExtension不是EX-GAS框架内脚本的，需要EX-GAS框架基础配置完成后，通过生成脚本生成。
- `static Type[] PresetAttributeSetTypes(this AbilitySystemComponent asc)`
  - 获取ASC的预设AttributeSet类型
  - 返回值：ASC的预设AttributeSet类型
- `static GameplayTag[] PresetBaseTags(this AbilitySystemComponent asc)`
  - 获取ASC的预设基础Tag
  - 返回值：ASC的预设基础Tag
- `static void InitWithPreset(this AbilitySystemComponent asc,int level, AbilitySystemComponentPreset preset = null)`
  - 通过预设初始化ASC
  - level：ASC的初始化等级
  - preset：ASC的预设

### 3.3 GameplayTag
#### 3.3.1 GameplayTag
GameplayTag是GAS的标签类，它是GAS的核心类。Tag的设计结构虽然简单，但是在实际应用中十分高效有用。
- `int HashCode => _hashCode;`
  - Tag的HashCode
- `string[] AncestorNames => _ancestorNames;`
  - Tag的父级名 
- `int[] AncestorHashCodes => _ancestorHashCodes;`
  - Tag的父级HashCode集合
- `bool Root => _ancestorHashCodes.Length == 0;`
  - Tag是否是根Tag 
- `bool IsDescendantOf(GameplayTag other)`
  - Tag是否是指定Tag的子Tag
  - other：指定Tag
  - 返回值：是否是子Tag
- `bool HasTag(GameplayTag tag)`
  - Tag是否持有指定Tag,比如‘Buff.Burning’ 持有 ‘Buff’
  - tag：指定Tag
  - 返回值：是否持有
#### 3.3.2 GameplayTagSet
GameplayTagSet是Tag集合类之一。GameplayTagSet适用于稳定不会改变的Tag集合。通常数据类的Tag集合都用GameplayTagSet。
- `readonly GameplayTag[] Tags`
  - Tag数据
- `bool Empty => Tags.Length == 0;`
  - Tag集合是否为空
- `bool HasTag(GameplayTag tag)`
  - TagSet是否持有指定Tag
  - tag：指定Tag
  - 返回值：是否持有
- `bool HasAllTags(GameplayTagSet other) / bool HasAllTags(params GameplayTag[] tags)`
  - TagSet是否持有指定Tag集合中的所有Tag
  - other：指定Tag集合
  - 返回值：是否持有
- `bool HasAnyTags(GameplayTagSet other) / bool HasAnyTags(params GameplayTag[] tags)`
  - TagSet是否持有指定Tag集合中的任意一个Tag
  - other：指定Tag集合
  - 返回值：是否持有
- `bool HasNoneTags(GameplayTagSet other) / bool HasNoneTags(params GameplayTag[] tags)`
  - TagSet是否不持有指定Tag集合中的所有Tag
  - other：指定Tag集合
  - 返回值：是否不持有

#### 3.3.3 GameplayTagContainer
GameplayTagContainer是Tag集合类之一。GameplayTagContainer适用于经常改变的Tag集合。
- `List<GameplayTag> Tags { get; }`
  - Tag数据
- `void AddTag(GameplayTag tag)`
  - 添加Tag
  - tag：指定Tag
- `void AddTag(GameplayTagSet tagSet)`
  - 添加Tag集合
  - tagSet：要添加的Tag集合
- `void RemoveTag(GameplayTag tag)` 
  - 移除Tag
  - tag：指定Tag
- `void RemoveTag(GameplayTagSet tagSet)`
  - 移除Tag集合
  - tagSet：要移除的Tag集合
- `bool HasTag(GameplayTag tag)`
  - TagContainer是否持有指定Tag
  - tag：指定Tag
- `bool HasAllTags(GameplayTagSet other) / bool HasAllTags(params GameplayTag[] tags)`
  - TagContainer是否持有指定Tag集合中的所有Tag
  - other：指定Tag集合
  - 返回值：是否持有
- `bool HasAnyTags(GameplayTagSet other) / bool HasAnyTags(params GameplayTag[] tags)`
  - TagContainer是否持有指定Tag集合中的任意一个Tag
  - other：指定Tag集合
  - 返回值：是否持有
- `bool HasNoneTags(GameplayTagSet other) / bool HasNoneTags(params GameplayTag[] tags)`
  - TagContainer是否不持有指定Tag集合中的所有Tag
  - other：指定Tag集合
  - 返回值：是否不持有

#### 3.3.4 GameplayTagAggregator
GameplayTagAggregator是专门针对ASC的Tag管理类，会针对固有Tag和动态Tag做不同的处理。
- `void Init(GameplayTag[] tags)`
  - 初始化
  - tags：初始化的固有Tag
- `void AddFixedTag(GameplayTag tag)`
  - 添加固有Tag
  - tag：添加的Tag
- `void AddFixedTag(GameplayTagSet tagSet)`
  - 添加固有Tag集合
  - tagSet：添加的Tag集合
- `void RemoveFixedTag(GameplayTag tag)`
  - 移除固有Tag
  - tag：移除的Tag
- `void RemoveFixedTag(GameplayTagSet tagSet)`
  - 移除固有Tag集合
  - tagSet：移除的Tag集合
- `void ApplyGameplayEffectDynamicTag(GameplayEffectSpec source)`
  - 从GameplayEffect中应用动态Tag（Granted Tags）
  - source：GameplayEffect的规格类实例
- `void ApplyGameplayAbilityDynamicTag(AbilitySpec source)`
  - 从Ability中应用动态Tag（Activation Owned Tags）
  - source：Ability的规格类实例
- `RestoreGameplayEffectDynamicTags(GameplayEffectSpec effectSpec)`
  - 从GameplayEffect中恢复动态Tag（Granted Tags）
  - effectSpec：GameplayEffect的规格类实例
- `RestoreGameplayAbilityDynamicTags(AbilitySpec abilitySpec)`
  - 从Ability中恢复动态Tag（Activation Owned Tags）
  - abilitySpec：Ability的规格类实例
- `bool HasTag(GameplayTag tag)` 
  - TagAggregator是否持有指定Tag
  - tag：指定Tag
  - 返回值：是否持有
- `bool HasAllTags(GameplayTagSet other) / bool HasAllTags(params GameplayTag[] tags)`
  - TagAggregator是否持有指定Tag集合中的所有Tag
  - other：指定Tag集合
  - 返回值：是否持有
- `bool HasAnyTags(GameplayTagSet other) / bool HasAnyTags(params GameplayTag[] tags)`
  - TagAggregator是否持有指定Tag集合中的任意一个Tag
  - other：指定Tag集合
  - 返回值：是否持有
- `bool HasNoneTags(GameplayTagSet other) / bool HasNoneTags(params GameplayTag[] tags)`
  - TagAggregator是否不持有指定Tag集合中的所有Tag
  - other：指定Tag集合
  - 返回值：是否不持有 
#### 3.3.5 GTagLib(Script-Generated Code)
GTagLib是GAS的标签库，它是GAS的标签管理类。
GTagLib不是EX-GAS框架内脚本的，需要EX-GAS框架Tag配置改动后，通过生成脚本生成。
- `public static GameplayTag XXX { get;} = new GameplayTag("XXX");`
- `public static GameplayTag XXX_YYY { get;} = new GameplayTag("XXX.YYY");`
  - GTagLib会把所有的Tag都生成为静态字段，方便外部调用。格式如上所示。
  - A.B.C的Tag会生成为A_B_C的静态字段。
- `public static Dictionary<string, GameplayTag> TagMap = new Dictionary<string, GameplayTag>
  {
  ["A"] = A,
  ["A.B"] = A_B,
  ["A.C"] = A_C,
  };`
  - GTagLib还包含了一个TagMap，方便外部通过Tag的字符串名来获取Tag。

### 3.4 Attribute
#### 3.4.1 AttributeBase
#### 3.4.2 AttributeValue
#### 3.4.3 AttributeAggregator
#### 3.4.4 DerivedAttribute(W.I.P)

### 3.5 AttributeSet
#### 3.5.1 AttributeSet
#### 3.5.2 AttributeSetContainer
#### 3.5.3 CustomAttrSet

### 3.6 GameplayEffect
#### 3.6.1 GameplayEffectAsset
#### 3.6.2 GameplayEffect
#### 3.6.3 GameplayEffectSpec
#### 3.6.4 GameplayEffectContainer
#### 3.6.5 GameplayEffectTagContainer
#### 3.6.6 CooldownTimer
#### 3.6.7 GameplayEffectModifier
##### 3.6.7.0 ModifierMagnitudeCalculation
##### 3.6.7.1 ScalableFloatModCalculation
##### 3.6.7.2 AttributeBasedModCalculation
##### 3.6.7.3 SetByCallerFromNameModCalculation
##### 3.6.7.4 SetByCallerFromTagModCalculation

### 3.7 Ability
#### 3.7.1 AbilityAsset
#### 3.7.2 AbstractAbility
#### 3.7.3 AbilitySpec
#### 3.7.4 AbilityTagContainer
#### 3.7.5 AbilityContainer
#### 3.7.6 AbilityTask

### 3.7.EX  Timeline Ability
#### 3.7.EX.1 TimelineAbilityAsset
#### 3.7.EX.2 TimelineAbility
#### 3.7.EX.3 TimelineAbilitySpec
#### 3.7.EX.4 TimelineAbilityPlayer
#### 3.7.EX.5 Target Catcher
##### 3.7.EX.5.1 TargetCatcherBase
##### 3.7.EX.5.2 CatchSelf
##### 3.7.EX.5.3 CatchTarget
##### 3.7.EX.5.4 CatchAreaBox2D
##### 3.7.EX.5.5 CatchAreaCircle2D

### 3.8 GameplayCue
#### 3.8.1 GameplayCue
#### 3.8.2 GameplayCueSpec
#### 3.8.3 GameplayCueParameters
#### 3.8.4 GameplayCueInstant
#### 3.8.5 GameplayCueDuration

---
## 4.可视化功能
### 1. GAS Setting Manager (GAS基础配置管理器)
![QQ20240313174500.png](Wiki%2FQQ20240313174500.png)
基础配置是与项目工程唯一对应的，所以入口放在了ProjectSetting，另外还有Edit Menu栏入口：EX-GAS -> Setting

- GameplayTag Manager
![QQ20240313114652.png](Wiki%2FQQ20240313114652.png)
- Attribute Manager
![QQ20240313115953.png](Wiki%2FQQ20240313115953.png)
- AttributeSet Manager
![QQ20240313121300.png](Wiki%2FQQ20240313121300.png)


### 2. GAS Asset Aggregator (GAS配置资源聚合器)
![QQ20240313175247.png](Wiki%2FQQ20240313175247.png)
因为GAS使用过程需要大量的配置（各类预设：ASC，游戏能力，游戏效果/buff，游戏提示，MMC），为了方便集中管理，我制作了一个配置资源聚合器。

通过在菜单栏EX-GAS -> Asset Aggregator 可以打开配置资源聚合器。

聚合器支持：分类管理，文件夹树结构显示，搜索栏快速查找，快速创建/删除配置文件（右上角的快捷按钮）
- ASC预设管理
![QQ20240313175513.png](Wiki%2FQQ20240313175513.png)
- 能力配置管理
![QQ20240313175749.png](Wiki%2FQQ20240313175749.png)
- 游戏效果管理
![QQ20240313175829.png](Wiki%2FQQ20240313175829.png)
- 游戏提示 & MMC 管理
![QQ20240313180028.png](Wiki%2FQQ20240313180028.png)
![QQ20240313180054.png](Wiki%2FQQ20240313180054.png)
### 3. GAS Runtime Watcher (GAS运行时监视器)
![QQ20240313180923.png](Wiki%2FQQ20240313180923.png)
__*注意！由于该监视器的监视刷新逻辑过于暴力，因此存在明显的性能问题。监视器只是为了方便调试，所以建议不要一直后台挂着监视器，有需要时再打开。*__

>目前监视器较为简陋，以后可能会优化监视器。

---

## 5.如果...我想...,应该怎么做?(W.I.P)

---
## 6.暂不支持的功能（可能有遗漏）
- Granted Ability，GameplayEffect授予的能力。虽然面板显示了配置用的字段，但目前其实是不生效的
- Derived Attribute，推论Attribute还未实现。
- GameplayEffect Stack， 同一游戏效果堆叠（如燃烧效果堆叠，伤害提升）
- RPC相关的GE复制广播
- GameplayEffect Execution，目前只有Modifier，没有Execution
- Ability的触发判断用的Source/Target Tag目前不生效

## 7.后续计划
- 修复bug ，性能优化
- 补全遗漏的功能 
- 优化Ability的编辑
- 支持RPC的GE复制广播，网络同步 
- 将GAS移交DOTS或采用ECS结构来运行(可能会做)

## 8.特别感谢
本插件全面参考了[UE的GAS解析](https://github.com/tranek/GASDocumentation)，来自github --[@tranek](https://github.com/tranek)

同时还有[中译版本](https://github.com/BillEliot/GASDocumentation_Chinese)，来自github --[@BillEliot](https://github.com/BillEliot)

没有上述二位的文章，本项目的开发会非常痛苦。

另外还要感谢开源项目：[UnityToolchainsTrick](https://github.com/XINCGer/UnityToolchainsTrick)

多亏UnityToolchainsTrick中的大量Editor开发技巧，极大的缩减了项目中编辑器的制作时间，省了很多事儿。非常感谢！
## 9.插件反馈渠道
QQ群号:616570103

目前该插件是一定有大量bug存在的，因为有非常多的细节没有测试到，虽然有Demo演示，但也只是一部分的功能。所以我希望有人能使用该插件，多多反馈，来完善该插件。

GAS使用门槛高，所以有任何GAS相关使用的疑问，bug或者建议，欢迎来反馈群里交流。我都会尽可能回答的。


