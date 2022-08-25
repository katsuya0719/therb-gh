---
id: Filter
title: Filter
sidebar_position: 3
---
データのフィルタリングを行うコンポーネントのカテゴリ

---

## FilterConstruction  
Constructionデータの中から条件に合致するConstructionデータを抽出します  
### 入力  
| パラメータ  | 説明                             | 型                    | required | 
| --------- | -------------------------------- | --------------------- | -------- | 
| Constructions     | フィルタリングするFaceモデル     | Face | Yes      | 
| category   | フィルタリングするcategoryの条件 | surface               | No       | 

### 出力  


## FilterFaceByProperty  
Faceモデルのリスト中から条件に合致するFaceモデルを抽出します  
### 入力  
| パラメータ  | 説明                             | 型                    | required | 
| --------- | -------------------------------- | --------------------- | -------- | 
| Faces     | フィルタリングするFaceモデル     | Face | Yes      | 
| bc   | フィルタリングするboundary condtionの条件 | surface               | No       | 
| surfT  | フィルタリングするsurface typeの条件 | surface               | No       | 
| direction | フィルタリングするdirectionの条件  | float                 | No       | 
