---
id: CreateTherbModel
title: Create THERB Model
---

RhinoにモデリングしたジオメトリデータをTHERBデータに変換し、サーバーにuploadするまでの一通りの流れを説明します。  

## Rhinoモデルの作成  
### 室データの作成  
1. 室ごとにpolysurface、もしくはextrusionをモデリングします  
1. 作成したジオメトリを全選択し、「explode」コマンドを打ちます  
1. 2.で作成したジオメトリをbrepコンポーネントに格納し、assignClassコンポーネントのgeosにつなぎます  

### 窓データの作成  
1. surfaceとしてモデリングします  
1. 1.で作成したジオメトリをsurfaceコンポーネントに格納し、assignClassコンポーネントのwindowsにつなぎます  

### 庇データの作成  
(現在準備中です)  

以上のデータを入力すると、THERBの計算に必要なb.datファイルとr.datファイルが自動的に生成されます  

(YouTubeの動画を埋め込む)  
