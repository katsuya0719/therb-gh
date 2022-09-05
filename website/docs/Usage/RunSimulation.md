---
id: RunSimulation
title: THERBシミュレーションを開始する
sidebar_position: 5
---

THERBのモデルが構築できたら、RunSimulationコンポーネントを使用して計算を実行できます。  

## RunSimulationコンポーネントへの入力  
RunSimulationコンポーネントを配置し、THERBモデルを入力します。  
nameにシミュレーションケースの名前を入れ、runにtrueを入力すると、ダイアログが表示されます。ダイアログでダウンロードしたデータの中にある「example/THERB」フォルダを選択します。ファイルに問題がなければ、コマンドプロンプト（黒色のスクリーン）が表示され、ファイルのシミュレーションが開始します。  

## 計算結果の確認  
計算が終了したら、[c://therb/{シミュレーションケース名}」フォルダのo.datファイルを開きます。このデータが計算結果データを保持しています。  

(フォーマットの読み方に関するドキュメントを誰かに書いてもらう)   

## 壁体構成を編集したい場合  
デフォルトの壁体構成が割り当てられています。この壁体構成を編集したい場合は[こちらのページ](./RegisterConstruction.md)を参照ください。


