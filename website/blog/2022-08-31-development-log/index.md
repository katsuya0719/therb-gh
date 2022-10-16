---
slug: development
title: 開発のログ1
authors:
  name: katsuya obara
  title: Lead Engineer at BeCAT
  url: https://github.com/katsuya0719
  image_url: https://github.com/katsuya0719.png
tags: [development]
---

打ち合わせしたい点  
- 全体のシステムに関する説明  
- クラウドで計算するTHERBのランタイムがo.datファイルに書き出す項目について  
- 想定する内壁の数について  
- 秋学期でのTHERBを使用するカリキュラムのイメージ  

例)簡単な建物モデルを自分で作ってTHERB-GHを使って計算  
窓の大きさ、形、壁体構成などを変更してみて、その結果を分析、考察する  

全体のシステムダイアグラム  
![THERB2.0](./system-diagram.png)  

出力項目  
- 外気温度
- 外気相対湿度  
- 外気絶対湿度  
- 室内温度  
- 相対湿度  
- 絶対湿度  
- 顕熱負荷  
- 潜熱負荷    

