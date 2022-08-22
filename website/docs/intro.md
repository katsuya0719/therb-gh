---
sidebar_position: 1
---

# チュートリアル

THERB-GHの使用方法の概略を説明します.

## Getting Started

1. [github](https://github.com/becat-oss/therb-gh)からソースコードをダウンロードします。  

2. THERB-GHをGrasshopperにインストールします。インストールの方法は[こちらのページ](Usage/HowToInstall.md)を参照ください。

3. ダウンロードしたファイルの中にあるexample/example.3dmファイルを開きます。  

4. Grasshopperを開き、example/example.ghファイルをGrasshopper上で開きます。  

5. Rhino上にRoomデータ、Windowデータ、Overhang(水平庇)データをモデリングし、[THERBコンポーネント](Component/Modelling.md)に格納します。詳しい方法は[こちらのページ](Usage/CreateTherbModel.md)を参照ください。  

6. b.dat,r.dat,w.datファイルが入力情報によって生成されるので、そのファイルをダウンロードしたデータのexample/test/THERB_formatフォルダ内に生成してください。  

7. コマンドプロンプトでexample/test/THERB_formatのフォルダにいき、そこでtherb.exeと入力し、そのあとt.datと入力すればtherbの計算が回ります。  
