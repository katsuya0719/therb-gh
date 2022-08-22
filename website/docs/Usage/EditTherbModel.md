---
id: EditTherbModel
title: THERBモデルを編集する
sidebar_position: 4
---

THERBモデルを編集する（例. 外壁を変更するなど）手順を示します。  
以下の例では、外壁の構成を変更する手順を説明します。  

### Faceモデルを取り出す    
THERBモデルはRoom,Face,Window,Overhangのモデルで構成されています。それぞれのモデルの詳細説明](../Detail/Concept.md)はこちらを参照ください。  
1. THERBモデルをDecomposeコンポーネントにつないで、Faceモデルを取り出す  

### 編集したいFaceモデルをフィルタリングする  
1. FaceモデルをFilterFaceByPropertyコンポーネントに接続し、「bc」「surfT」「direction」を選択し、条件に合致したFaceを抽出する  

### 外壁構成を上書きする  
1. ReadConstructionコンポーネントを配置し、サーバーから外壁構成データを取得する  
2. UpdateConstructionコンポーネントに外壁構成を変更したいFaceを接続し、constructionデータを選択することでFaceのconstructionデータが上書きされます。  

### THERBモデルを再構築する  
編集するために分解したRoom,Face,Window,Overhangモデルをcomposeコンポーネントに接続し、THERBモデルを再構築します。  

次のステップ  
[シミュレーションを行う](./RunSimulation.md)