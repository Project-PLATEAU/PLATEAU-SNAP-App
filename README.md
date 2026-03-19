# PLATEAU-SNAP-App

<img src="docs/images/manual/index.webp" width="300">

## 1. 概要

本リポジトリでは、2024～2025年度に Project PLATEAU で開発した「PLATEAU-SNAP-App」のソースコードを公開しています。  
「PLATEAU-SNAP-App」は、スマートフォンで撮影した画像から3D都市モデルへのテクスチャ付与を行うシステム (PLATEAU SNAP) のモバイルアプリケーションです。

## 2. PLATEAU-SNAP-App について

スマートフォンで撮影した画像をもとに 3D 都市モデルの建築物のテクスチャ(地物の外観)を抽出・生成し、PLATEAU SNAPのデータベースに登録・蓄積可能なツールを開発しました。

本システムは、PLATEAU SNAP のテクスチャ用画像の撮影ツールです。スマートフォンの位置や姿勢を高精度で推定する VPS 機能に加えて、撮影可能な建物の面をカメラ映像上に AR 表示する機能や撮影した画像の品質を評価する機能、撮影画像をアップロードする機能を提供します。
技術詳細は、ProjectPLATEAU公式ホームページ内に技術検証レポートとして記載をしています。

各リポジトリの役割は以下の通りです。

-   **PLATEAU-SNAP-App**: 画像の撮影・アップロード
-   **[PLATEAU-SNAP-Server](https://github.com/Project-PLATEAU/PLATEAU-SNAP-Server)**: データの蓄積、画像処理、モデル生成（バックエンド処理）
-   **[PLATEAU-SNAP-CMS](https://github.com/Project-PLATEAU/PLATEAU-SNAP-CMS)**: テクスチャの生成・貼り付け、データベース更新、データ出力

  尚、本スマートフォンアプリを利用するためにビルドが必要なのは、**PLATEAU-SNAP-App**のみです。

### データの取り扱いについて
本ツールにより登録されたテクスチャデータは、PLATEAU SNAP のデータベースに蓄積されますが、
G空間情報センターや PLATEAU VIEW で公開されている 3D 都市モデル自体を直接更新するものではありません。
作成したテクスチャデータはエクスポートすることが可能であり、独自の用途で活用することが可能です。  
なお、本ツールで生成したデータを独自利用する場合の取り扱いや公開可否等については、必要に応じて Project PLATEAU へご確認ください。  
問い合わせ先については、[Project PLATEAUの公式窓口](https://www.mlit.go.jp/plateau/contact/)をご参照ください。



## 3. 利用手順

本システムの構築手順及び利用手順については[操作マニュアル](https://project-plateau.github.io/PLATEAU-SNAP-App/index.html)を参照してください。

## 4. システム概要

| 機能名               | 機能説明                                                                                   |
| -------------------- | ------------------------------------------------------------------------------------------ |
| VPS 機能             | カメラ映像を解析し、周囲の建物やランドマークを基にカメラの位置や向きを高精度に推定する機能 |
| AR での面表示機能    | クラウドから撮影可能な建物の面を取得し、カメラ映像上に AR 表示する機能                     |
| 建物撮影機能         | AR で表示された建物の面を選択し、その面の画像を撮影する機能                                |
| 品質評価機能         | 撮影した画像がテクスチャ素材として適切かどうか評価する機能                                 |
| 画像アップロード機能 | 撮影画像と貼り付け先の面情報をデータベースにアップロードする機能                           |
| 正射変換機能　　　　 | 斜めから撮影した建物画像を対象面に対して正面から撮影したように補正する機能                           |


## 5. 利用技術

| 項目                 | 名称  | バージョン                                                           | 内容                       |
| -------------------- | ----- | -------------------------------------------------------------------- | -------------------------- |
| 開発プラットフォーム | Unity | [2022.3.44f1](https://docs.unity3d.com/ja/2022.3/Manual/)            | モバイルアプリの開発に使用 |
| 使用言語             | C#    | [9.0](https://docs.unity3d.com/ja/2022.3/Manual/CSharpCompiler.html) | Unity のサポート言語       |

### Unity 関連のパッケージ

-   ランタイム
    -   ランタイムの実行に必要な Unity の依存パッケージです。

| 名称                                                                       | バージョン                                                                                 | 内容                               |
| -------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------ | ---------------------------------- |
| [PLATEAU.SNAP.Server](https://github.com/Project-PLATEAU/PLATEAU-SNAP-Server) | 0.0.1                                                                                      | SNAP のサーバーサイド              |
| AR Foundation                                                              | [5.1.5](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/manual/index.html) | AR 機能実装に使用                  |
| Google ARCore XR Plugin                                                    | [5.1.5](https://docs.unity3d.com/ja/Packages/com.unity.xr.arkit@5.1/manual/index.html)     | AR 機能実装(Geospatial)に使用      |
| ARCore Extensions                                                          | [1.22.3](https://github.com/google-ar/arcore-unity-extensions)                             | AR 機能の拡張を提供(ARCore が依存) |
| UniTask                                                                    | [2.5.10](https://github.com/Cysharp/UniTask/releases/tag/2.5.10)                           | 非同期タスクの実装に使用           |
| R3                                                                         | [1.2.9](https://github.com/Cysharp/R3/releases/tag/1.2.9)                                  | リアクティブな実装に使用           |
| Geometry                                                                   | [0.0.5](https://github.com/iShapeUnity/Geometry/releases/tag/0.0.5)                        | メッシュ描画に使用                 |
| Mesh2d                                                                     | [0.0.9](https://github.com/iShapeUnity/Mesh2d/releases/tag/0.0.9)                          | メッシュ描画に使用                 |
| Triangulation                                                              | [0.0.8](https://github.com/iShapeUnity/Triangulation/releases/tag/0.0.8)                   | メッシュ描画に使用                 |

-   サンプル
    -   サンプルの実行に必要な Unity の依存パッケージです。
    -   ランタイムに併せて必要です。

| 名称         | バージョン                                                                                | 内容                                         |
| ------------ | ----------------------------------------------------------------------------------------- | -------------------------------------------- |
| Addressables | [1.22.3](https://docs.unity3d.com/Packages/com.unity.addressables@1.22/manual/index.html) | リソース管理に使用                           |
| Localization | [1.5.4](https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/index.html)   | 文字列のローカライズに使用                   |
| TextMeshPro  | [3.0.6](https://docs.unity3d.com/ja/2022.3/Manual/com.unity.textmeshpro.html)             | 文字列 UI 描画(メッシュベース)に使用         |
| VContainer   | [1.16.8](https://github.com/hadashiA/VContainer/releases/tag/1.16.8)                      | Dependency Injection に使用                  |
| Polly        | [8.5.2](https://www.nuget.org/packages/Polly/8.5.2)                                       | OpenAPI で生成されたコードのコンパイルに必要 |

## 6. 動作環境

-   テストデバイス
    -   iPhone 13 Pro（iOS 18.3.1）
-   推奨環境
    -   Geospatial API に対応している[iOS デバイス](https://developers.google.com/ar/devices?hl=ja#ios)（※すべてのデバイスでの動作を保証するものではありません。）
    -   最新の iOS バージョン
-   ネットワーク要件
    -   本システムは 3D 都市モデルの座標情報の取得および画像データの送信のため、インターネット接続が必要です。

## 7. 本リポジトリのフォルダ構成

| フォルダ名   | 詳細                       | 関連情報                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| ------------ | -------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| SnapForUnity | Unity プロジェクト         | [構成](<https://github.com/Synesthesias/PLATEAU-SNAP-App/wiki/1).-%E9%96%8B%E7%99%BA%E8%80%85(Developer)%E5%90%91%E3%81%91%E6%83%85%E5%A0%B1#1-1-%E6%A7%8B%E6%88%90>)                                                                                                                                                                                                                                                                                                        |
| .config      | .NET の環境設定            | [クライアントの API ドキュメントの作成方法](<https://github.com/Synesthesias/PLATEAU-SNAP-App/wiki/2).-%E3%82%B3%E3%83%B3%E3%83%88%E3%83%AA%E3%83%93%E3%83%A5%E3%83%BC%E3%82%BF%E3%83%BC(Contributer)%E5%90%91%E3%81%91%E6%83%85%E5%A0%B1#2-3-%E3%82%AF%E3%83%A9%E3%82%A4%E3%82%A2%E3%83%B3%E3%83%88%E3%81%AEapi%E3%83%89%E3%82%AD%E3%83%A5%E3%83%A1%E3%83%B3%E3%83%88%E3%81%AE%E4%BD%9C%E6%88%90%E6%96%B9%E6%B3%95%E8%BF%BD%E5%8A%A0%E5%AF%BE%E5%BF%9C%E4%B8%8D%E8%A6%81>)  |
| .github      | GitHub の設定ファイル      | [プルリクエストの作成手順](<https://github.com/Synesthesias/PLATEAU-SNAP-App/wiki/2).-%E3%82%B3%E3%83%B3%E3%83%88%E3%83%AA%E3%83%93%E3%83%A5%E3%83%BC%E3%82%BF%E3%83%BC(Contributer)%E5%90%91%E3%81%91%E6%83%85%E5%A0%B1#2-6-%E3%83%97%E3%83%AB%E3%83%AA%E3%82%AF%E3%82%A8%E3%82%B9%E3%83%88%E3%81%AE%E4%BD%9C%E6%88%90%E6%89%8B%E9%A0%86>)                                                                                                                                  |
| OpenAPI      | API 通信用のコード生成関連 | [サンプルの API 通信用のクライアントコードの更新方法](<https://github.com/Synesthesias/PLATEAU-SNAP-App/wiki/2).-%E3%82%B3%E3%83%B3%E3%83%88%E3%83%AA%E3%83%93%E3%83%A5%E3%83%BC%E3%82%BF%E3%83%BC(Contributer)%E5%90%91%E3%81%91%E6%83%85%E5%A0%B1#2-5-%E3%82%B5%E3%83%B3%E3%83%97%E3%83%AB%E3%81%AEapi%E9%80%9A%E4%BF%A1%E7%94%A8%E3%81%AE%E3%82%AF%E3%83%A9%E3%82%A4%E3%82%A2%E3%83%B3%E3%83%88%E3%82%B3%E3%83%BC%E3%83%89%E3%81%AE%E6%9B%B4%E6%96%B0%E6%96%B9%E6%B3%95>) |
| docs         | アプリの API ドキュメント  | [クライアントの API ドキュメントの作成方法](<https://github.com/Synesthesias/PLATEAU-SNAP-App/wiki/2).-%E3%82%B3%E3%83%B3%E3%83%88%E3%83%AA%E3%83%93%E3%83%A5%E3%83%BC%E3%82%BF%E3%83%BC(Contributer)%E5%90%91%E3%81%91%E6%83%85%E5%A0%B1#2-3-%E3%82%AF%E3%83%A9%E3%82%A4%E3%82%A2%E3%83%B3%E3%83%88%E3%81%AEapi%E3%83%89%E3%82%AD%E3%83%A5%E3%83%A1%E3%83%B3%E3%83%88%E3%81%AE%E4%BD%9C%E6%88%90%E6%96%B9%E6%B3%95%E8%BF%BD%E5%8A%A0%E5%AF%BE%E5%BF%9C%E4%B8%8D%E8%A6%81>)  |

## 8. ライセンス

-   ソースコード及び関連ドキュメントの著作権は国土交通省に帰属します。
-   本ドキュメントは[Project PLATEAU のサイトポリシー](https://www.mlit.go.jp/plateau/site-policy/)（CCBY4.0 及び政府標準利用規約 2.0）に従い提供されています。

## 9. 注意事項

-   本リポジトリは参考資料として提供しているものです。動作保証は行っていません。
-   本リポジトリについては予告なく変更又は削除をする可能性があります。
-   本リポジトリの利用により生じた損失及び損害等について、国土交通省はいかなる責任も負わないものとします。

## 10. 参考資料

-   [技術検証レポート](https://www.mlit.go.jp/plateau/file/libraries/doc/plateau_tech_doc_0139_ver01.pdf)
