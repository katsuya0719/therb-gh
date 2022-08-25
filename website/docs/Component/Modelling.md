---
id: Modelling
title: Modelling
sidebar_position: 1
---
THERBデータの生成、編集を行うコンポーネントのカテゴリ

---

## THERB  
THERBモデルを生成する。THERBモデルの詳細説明に関しては[こちらのページ](../Detail/Concept.md)をご覧ください。  

### 入力  

| パラメータ  | 説明                             | 型                    | required | 
| --------- | -------------------------------- | --------------------- | -------- | 
| boxes     | 室に相当するジオメトリを入力     | extrusion/polysurface | Yes      | 
| windows   | 窓に相当するジオメトリを入力     | surface               | No       | 
| overhang  | 水平庇に相当するジオメトリを入力 | surface               | No       | 
| tolerance | 内壁判定の基準となる値を入力     | float                 | No       | 

### 出力  

---
## Decompose  
THERBモデルをRoom,Face,Window,Overhangモデルに分解します。分解したFace,Windowにconstructionを適用することで壁体構成を編集することができます。詳細は[こちらのページ](../Usage/EditTherbModel.md)  

### 入力  
| パラメータ  | 説明                             | 型                    | required | 
| --------- | -------------------------------- | --------------------- | -------- | 
| Therb     | 室に相当するジオメトリを入力     | extrusion/polysurface | Yes      | 
| windows   | 窓に相当するジオメトリを入力     | surface               | No       | 
| overhang  | 水平庇に相当するジオメトリを入力 | surface               | No       | 
| tolerance | 内壁判定の基準となる値を入力     | float                 | No       | 


## Compose  
Room,Face,Window,Overhangモデルを統合して、THERBモデルにします。  
編集したFace,Windowをもう一度結合して[RunSimulation](./Simulation.md)につなげるために使用します。  

### 入力  
| パラメータ  | 説明                             | 型                    | required | 
| --------- | -------------------------------- | --------------------- | -------- | 
| Rooms     | Roomモデルを入力     | Room | Yes      | 
| Faces     | Faceモデルを入力     | Face               | No       | 
| Windows   | Windowモデルを入力 | Window               | No       | 
| Overhang  | Overhangモデルを入力     | Overhang                 | No       | 

## UpdateConstruction  
Faceモデルに紐づけるconstructionデータ(壁体構成情報)を更新します。  

### 入力  
| パラメータ  | 説明                             | 型                    | required | 
| --------- | -------------------------------- | --------------------- | -------- | 
| Faces     | Faceモデルを入力     | Face | Yes      | 
| Construction     | Constructionモデルを入力     | Construction               | Yes       | 

