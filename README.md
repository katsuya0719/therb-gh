# therb-gh.  

## 概要  
THERB-GHは、THERB(HASP)用のシミュレーション入力ファイルをRhinocerosに入力したジオメトリを元に生成するGrasshopperライブラリです。  

THERB-GHはTHERB-webのモデリング部分です。  
![THERB](https://user-images.githubusercontent.com/90674244/142145820-0d25d627-ebec-4a77-b8c4-75bc51a68175.png)　　

## インストール方法

THERB-GH のインストール方法について紹介します。

1. zipファイルをダウンロード
1. ダウンロードした zip ファイルを右クリックしプロパティから全般のタブの中にあるセキュリティの項目を「許可する」にし、zip を解凍する
1. Grasshopper を起動して File > Special Folders > Components folder を選択することで表示されるフォルダに、1. でダウンロードしたフォルダの中にある 「installer/THERB-GH.gha」 という名前のファイルを入れる
1. Rhino を再起動する  
1. 1でダウンロードしたフォルダの中にある「example/example.3dm」というRhinoファイルを開く  
1. Grasshopperを起動する  
1. 「example/example.gh」ファイルをGrasshopper上で開く  

## 使い方  
[ドキュメンテーションページ](https://katsuya0719.github.io/therb-gh/docs/Usage/CreateTherbModel)に詳細が書いてあります  

## 注意事項  
下図のようにジオメトリ同士をsplitしたあとに、L字のsurfaceができるようなモデルだと計算精度に誤差が出てしまいます。   
![image (1)](https://user-images.githubusercontent.com/10389953/177903263-cafc0a06-d641-4223-ace6-25af821f42c5.png)
