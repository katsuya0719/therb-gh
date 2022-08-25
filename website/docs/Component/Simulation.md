---
id: Simulation
title: Simulation
sidebar_position: 4
---
（守田君にフォーマットをお願いしたい）
シミュレーションの実行に関わるコンポーネントのカテゴリ

---
## RunSimulation  
THERBモデルをもとにシミュレーションを開始します。  

### 入力  
| パラメータ     | 説明                                               | 型           | required | 
| -------------- | -------------------------------------------------- | ------------ | -------- | 
| Therb          | シミュレーションを行うTherbモデルを入力            | THERB        | Yes      | 
| Constructions  | シミュレーションに適用するConstructionモデルを入力 | Construction | Yes      | 
| name           | シミュレーションケースの名前を入力                 | string       | Yes      | 
| run            | trueを入力すると計算を開始                         | boolean      | Yes      | 