---
sidebar_position: 1
---

# チュートリアル

THERB-GHの使用方法の概略を説明します.

## Getting Started
とりあえず、therbの計算を回してみる  
1. [github](https://github.com/becat-oss/therb-gh)からソースコードをダウンロードします。  

2. THERB-GHをGrasshopperにインストールします。インストールの方法は[こちらのページ](Usage/HowToInstall.md)を参照ください。

3. ダウンロードしたファイルの中にあるexample/example.3dmファイルを開きます。  

4. Grasshopperを開き、example/example.ghファイルをGrasshopper上で開きます。  

5. Rhino上にRoomデータ、Windowデータ、Overhang(水平庇)データをモデリングし、[THERBコンポーネント](Component/Modelling.md)に格納します。詳しい方法は[こちらのページ](Usage/CreateTherbModel.md)を参照ください。  

6. b.dat,r.dat,w.datファイルがstep3に表示されるので、エラーが出ていないかチェックしてください。

7. step4でプロジェクト名を入力し、runをtrueにすると、シミュレーションが開始します。詳細は[こちらのページ](Usage/RunSimulation.md)を参照ください。 

## Next level  
壁体構成を編集してみる  
1. 使用したい壁体構成をwebアプリに登録します。詳細は[こちらのページ](Usage/RegisterConstruction.md)を参照ください。  

2. 編集するFace(壁、床、天井の総称）を選択  
example/example.ghのstep2のFilterFaceByPropertyのbc,surfT,directionを右クリックし、フィルタリングしたい条件を選択します。  

3. 2で選択したFaceに適用するconstructionを選択します。  

4. ComposeコンポーネントのアウトプットのTherbをstep3のexportB,exportR,exportW、step4のRunSimulationコンポーネントにつなぎます。  

6. b.dat,r.dat,w.datファイルがstep3に表示されるので、エラーが出ていないかチェックしてください。

7. step4でプロジェクト名を入力し、runをtrueにすると、シミュレーションが開始します。詳細は[こちらのページ](Usage/RunSimulation.md)を参照ください。 
