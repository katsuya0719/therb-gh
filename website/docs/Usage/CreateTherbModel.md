---
id: CreateTherbModel
title: THERBモデルを作成する
sidebar_position: 2
---

Rhinoにジオメトリをモデリングし、THERBモデルへ変換する手順を示します。   

## THERBコンポーネントへの入力   
THERBコンポーネントをGrasshopperのキャンバス上に配置し、以下のジオメトリを準備、入力します。  
### 室データ(boxes)の作成  
1. 室ごとにpolysurface、もしくはextrusionをモデリングします  
2. Brepコンポーネントに格納し、THERBコンポーネントのboxesにつなぎます  

### 窓データ(windows)の作成  
1. surfaceとしてモデリングします   
2. Surfaceコンポーネントに格納し、THERBコンポーネントのwindowsにつなぎます  

### 庇データ(overhangs)の作成  
1. surfaceとしてモデリングします   
2. Surfaceコンポーネントに格納し、THERBコンポーネントのoverhangsにつなぎます  

:::note
全てのジオメトリがx,yの値ともに＋となるような座標でモデリングしてください。  
窓と庇データの入力は任意です。  
:::


(トラブルシューティングも盛り込むべき)
以上を行うと、Therbモデルが生成されます。

これで、THERBの計算を行う準備が整ったので、[シミュレーションの実行](./RunSimulation.md)に進みます。  
