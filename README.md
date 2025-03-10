# PLATEAU-SNAP-App

## 1. 概要

デジタルツインの実現に向けたクラウドソーシング型3D都市モデル作成システム(PLATEAU SNAP)のモバイルアプリケーションです。

## 2. PLATEAU-SNAP-Appについて

TODO: 追記予定

## 3. 利用手順

- 本リポジトリのWikiの[利用手順](https://github.com/Synesthesias/PLATEAU-SNAP-App/wiki/%E5%88%A9%E7%94%A8%E6%89%8B%E9%A0%86)をご確認ください。

## 4. システム概要

TODO: 追記予定

## 5. 利用技術

- 開発プラットフォーム
  - Unity [2022.3.44f1](https://unity.com/ja/releases/editor/archive)

### Unity関連のパッケージ

#### ランタイム

ランタイムの実行に必要なUnityの依存パッケージです。

| 名称 | バージョン | 内容 |
|-----|-------|-------|
| [PLATEAU.SNAP.Server](https://github.com/Synesthesias/PLATEAU-SNAP-Server) | 0.0.1 | SNAPのサーバーサイド |
| AR Foundation | [5.1.5](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/manual/index.html) | AR機能実装に使用 |
| Google ARCore XR Plugin | [5.1.5](https://docs.unity3d.com/ja/Packages/com.unity.xr.arkit@5.1/manual/index.html) | AR機能実装(Geospatial)に使用 |
| ARCore Extensions | [1.22.3](https://github.com/google-ar/arcore-unity-extensions) | AR機能の拡張を提供(ARCoreが依存) |
| UniTask | [2.5.10](https://github.com/Cysharp/UniTask/releases/tag/2.5.10) | 非同期タスクの実装に使用 |
| R3 | [1.2.9](https://github.com/Cysharp/R3/releases/tag/1.2.9) | リアクティブな実装に使用 |
| Geometry | [0.0.5](https://github.com/iShapeUnity/Geometry/releases/tag/0.0.5) | メッシュ描画に使用 |
| Mesh2d | [0.0.9](https://github.com/iShapeUnity/Mesh2d/releases/tag/0.0.9) | メッシュ描画に使用 |
| Triangulation | [0.0.8](https://github.com/iShapeUnity/Triangulation/releases/tag/0.0.8) | メッシュ描画に使用 |

#### サンプル

サンプルの実行に必要なUnityの依存パッケージです。(ランタイムに併せて必要です)

| 名称 | バージョン | 内容 |
|-----|-------|-------|
| Addressables | [1.22.3](https://docs.unity3d.com/Packages/com.unity.addressables@1.22/manual/index.html) | リソース管理に使用 |
| Localization | [1.5.4](https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/index.html) | 文字列のローカライズに使用 |
| TextMeshPro | [3.0.6](https://docs.unity3d.com/ja/2022.3/Manual/com.unity.textmeshpro.html) | 文字列UI描画(メッシュベース)に使用 |
| VContainer | [1.16.8](https://github.com/hadashiA/VContainer/releases/tag/1.16.8) | Dependency Injectionに使用 |
| Polly | [8.5.2](https://www.nuget.org/packages/Polly/8.5.2) | OpenAPIで生成されたコードのコンパイルに必要 |

## 6. 動作環境

- [サポートされているデバイスモデル一覧](https://developers.google.com/ar/devices?hl=ja)
  - iPhoneまたはAndroid端末で「Geospatial API をサポートしていない」と<b>記載されていない</b>端末であればサポートされています
- ネットワーク
  - 本アプリは建物の座標情報の受信と画像データの送信にモバイル通信を使用します

## 7. 本リポジトリのフォルダ構成

| フォルダ名 | 詳細 |
|-----|-----|
| SnapForUnity | Unityプロジェクト |
| .config | .NETの環境設定 |
| .github | GitHubの設定ファイル |
| OpenAPI | API通信用のコード生成関連 |
| docs | アプリのAPIドキュメント |

## 8. ライセンス

- [ライセンス](https://github.com/Synesthesias/PLATEAU-SNAP-App/blob/main/LICENSE)

## 9. 注意事項

- 動作確認はiOS端末でのみ行なっています
- Android端末での動作確認は行なっていないため動作は保証しません
- 本リポジトリについては予告なく変更又は削除をする可能性があります

## 10. 参考資料

- [Wiki](https://github.com/Synesthesias/PLATEAU-SNAP-App/wiki)
