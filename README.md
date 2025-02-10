# PLATEAU-SNAP-App
デジタルツインの実現に向けたクラウドソーシング型3D都市モデル作成システム(PLATEAU SNAP)のモバイルアプリケーション

## Runtime

### Version

[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=PLATEAU&nbsp;SNAP&message=0.0.1&color=0e6da0)](https://github.com/Gentlymad-Studios/PackageManagerTools)

### Support

[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Unity&message=2022.3.44f1%20or%20higher&color=0e6da0)](https://github.com/Gentlymad-Studios/PackageManagerTools)

### Dependencies

[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=AR%20Foundation&message=5.1.5&color=0e6da0)](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Google%20ARCore%20XR%20Plugin&message=5.1.5&color=0e6da0)](https://docs.unity3d.com/ja/Packages/com.unity.xr.arkit@5.1/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=ARCore%20Extensions&message=1.22.3&color=0e6da0)](https://github.com/google-ar/arcore-unity-extensions)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=UniTask&message=2.5.10&color=0e6da0)](https://github.com/Cysharp/UniTask/releases/tag/2.5.10)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=R3&message=1.2.9&color=0e6da0)](https://github.com/Cysharp/R3/releases/tag/1.2.9)

### Installation

Package/manifest.jsonに以下を追記します。
(サンプルの場合は後述)

```json
{
  "dependencies": {
    "jp.synesthesias.plateau.snap": "https://github.com/Synesthesias/PLATEAU-SNAP-App.git?path=SnapForUnity/Assets/Synesthesias.Snap",
    "com.cysharp.r3": "1.2.9",
    "com.cysharp.unitask": "2.5.10",
    "org.nuget.r3": "1.2.9",
    "com.google.ar.core.arfoundation.extensions": "https://github.com/google-ar/arcore-unity-extensions.git#1.47.0",
  },
  "scopedRegistries": [
    {
      "name": "OpenUPM",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.cysharp",
        "org.nuget"
      ]
    }
  ]
}
```

## Sample

### Dependencies

[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Addressables&message=1.22.3&color=0e6da0)](https://docs.unity3d.com/Packages/com.unity.addressables@1.22/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Localization&message=1.5.4&color=0e6da0)](https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=TextMeshPro&message=3.0.6&color=0e6da0)](https://docs.unity3d.com/ja/2022.3/Manual/com.unity.textmeshpro.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=VContainer&message=1.16.8&color=0e6da0)](https://github.com/hadashiA/VContainer/releases/tag/1.16.8)

### Installation

PackageManagerからSampleを追加する場合は上記のRuntime側のInstallationの対応に加えて更にPackage/manifest.jsonに以下を追記します。

```json
{
  "dependencies": {
    "com.unity.addressables": "1.22.3",
    "com.unity.localization": "1.5.4",
    "com.unity.textmeshpro": "3.0.6",
    "jp.hadashikick.vcontainer": "https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer#1.16.8",
  }
}
```

## サンプルを更新する場合の注意点

Unityパッケージの仕様により、サンプルファイルは Samples~ ディレクトリ(隠しディレクトリ)に配置されています。この仕様は [Unity公式ドキュメント](https://docs.unity3d.com/ja/2022.3/Manual/cus-samples.html)に従ったもので、Samples ディレクトリにリネームする代わりに、シンボリックリンクを活用して開発の効率化を図っています。

### Windowsユーザー向けシンボリックリンクの設定手順

Windowsユーザーの方はリポジトリ内で以下のコマンドを実行し、シンボリックリンクを有効化してください。

```bash
git config core.symlinks true
```

Macユーザーは特別な対応は不要です。

## ARCoreの設定方法 (iOS)

- Project Settings > XR Plugin-in Management > ARCore Extensions
- iOS Support Enabledにチェックを入れます
- Geospatialにチェックを入れます

# APIキーの管理に注意してください

- APIキーはgitで管理しないようにしください
- APIキーをアプリに組み込まないようにしてください
- APIキーはあくまで暫定対応です
  - Android: Keylessを使用してください
  - iOS: Authentication Tokenを使用してください
- APIキーを間違ってコミットしないように以下の設定ファイルは.gitignoreで除外しています

```
/ProjectSettings/ARCoreExtensionsProjectSettings.json
```

### ARCoreのAPIキーの設定方法

- Project Settings > XR Plugin-in Management > ARCore Extensions
- iOS Authentication StrategyをAPI Keyに設定します
  - 本来であればAuthentication Tokenを使用することを推奨します
  - iOS API KeyにAPIキーを設定します

## クライアントのコード生成の方法

```bash
// openapi-generatorのインストール
brew install openapi-generator

// OpenAPIのディレクトリへ移動
cd path/to/OpenAPI

// コードど削除
rm -rf ./Generated.Client/

// コードを生成
openapi-generator generate -i spec.json -g csharp -c config_client.json -o ./Generated.Client
```

### README.mdで依存パッケージの確認

- コードが生成できたら `Generated.Client` ディレクトリの中にある README.md の Dependencies セクションを確認してパッケージの依存を確認します

```markdown
## Dependencies

- [Json.NET](https://www.nuget.org/packages/Newtonsoft.Json/) - 13.0.2 or later
- [System.ComponentModel.Annotations](https://www.nuget.org/packages/System.ComponentModel.Annotations) - 5.0.0 or later
```

README.mdからは以下の2つのパッケージのバージョンの依存があることが判明

- Json.NET (Newtonsoft.Json)
    - 13.0.2以降
- System.ComponentModel.Annotations
    - 5.0.0以降

実際は、`using Polly` の箇所でコンパイルエラーになるため `Polly` のパッケージ名とバージョンを後述の手順で確認します

### openupm-cliでパッケージ名と提供バージョンを確認する

- 例えば `Polly` のパッケージ名とバージョンを確認は以下のような流れで行います

```bash
// npxをインストールする
brew install npx

// 検索でパッケージ名を確認する
// (初回の場合はyでopenupm-cliコマンドをインストールする)
npx openupm-cli search Polly

// パッケージのバージョン一覧を調べる
npx openupm-cli info org.nuget.polly
```

- `npx openupm-cli search Polly` を実行するとパッケージ名は `org.nuget.polly` であることが判明します
- `npx openupm-cli info org.nuget.polly` を実行すると `org.nuget.polly` の最新バージョンが `8.5.2` であることが判明します

### manifest.jsonに依存パッケージを記載する

- 自動生成される `packages-lock.json` 側に依存パッケージが既に導入されているか確認します
- packages-lock.jsonに記載がないことが確認できたら `manifest.json` に各種依存パッケージを記載します

- 例: Pollyの場合

```json
{
  "dependencies": {
    "org.nuget.polly": "8.5.2"
  }
}
```

### packages-lock.jsonを確認する

- 自動生成される `packages-lock.json` 側に依存パッケージが導入されているか確認します
- 今回の場合、 `org.nuget.polly` を導入したことにより前述のREADME.mdのDependenciesに記載があった `Annotation` が `packages-lock.json` に自動的に追記されていることが分かります

```json
{
  "dependencies": {
    "org.nuget.polly": {
      "version": "8.5.2",
      "depth": 0,
      "source": "registry",
      "dependencies": {
        "org.nuget.polly.core": "8.5.2"
      },
      "url": "https://package.openupm.com"
    },
    "org.nuget.polly.core": {
      "version": "8.5.2",
      "depth": 1,
      "source": "registry",
      "dependencies": {
        "org.nuget.microsoft.bcl.asyncinterfaces": "6.0.0",
        "org.nuget.microsoft.bcl.timeprovider": "8.0.0",
        "org.nuget.system.componentmodel.annotations": "4.5.0",
        "org.nuget.system.threading.tasks.extensions": "4.5.4"
      },
      "url": "https://package.openupm.com"
    }
  }
}
```

## プルリクエストの作成手順

- Githubのリポジトリの画面上部から `Pull requests` タブを選択します
- pushしたブランチの `Compare & Pull request` または `New pull request` からブランチを選択してから `Create pull request` を選択してください

### 1). 機能実装のプルリクエストの場合

- 表示されたテンプレートをそのまま使ってください
- (またはURLの末尾に `?template=feature_template.md` を追記してください)

### 2). 不具合修正のプルリクエストの場合

- URLの末尾に `?template=fix_template.md` を追記してください

### 3). その他のプルリクエストの場合

- テンプレートは作成していないです
- 必要に応じて追加してください
