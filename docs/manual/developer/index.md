# 2). 開発者(Developer)向け情報

本アプリのRuntimeとSample(後述)を利用してアプリの開発を行う開発者(Developer)向けの情報です。

## 2-1). 構成

本アプリはUnity(開発プラットフォーム)で開発しています。
以下の構成になっています。

- [Runtime](https://github.com/Project-PLATEAU/PLATEAU-SNAP-App/tree/main/SnapForUnity/Assets/Synesthesias.Snap/Runtime) (ランタイム)
  - アプリ開発に必要なランタイムが含まれています
  - アーキテクチャはMVP (Model View Presenter)を採用していますがPresenterは含まれていません
- [Sample](https://github.com/Project-PLATEAU/PLATEAU-SNAP-App/tree/main/SnapForUnity/Assets/Synesthesias.Snap/Samples~/Scripts) (サンプル)
  - UI、シーン等のリソースを含むサンプルアプリです
  - 前述のランタイムが必要です
  - アーキテクチャはMVP (Model View Presenter)を採用しています

```bash
Runtime/
├── Model/
└── View/
```

```bash
Samples/Scripts/
├── Define/ (定数やenumを含む定義関連)
├── Generated/ (OpenAPIで生成したソースコード)
├── LifetimeScope/ (VContainerでのDI)
├── Model/ (MVPのModel)
├── Parameter/ (シーン間でのパラメータの受け渡し)
├── Presenter/ (MVPのPresenter)
├── Repository/ (データの永続化 - シーン間でのデータの受け渡し用)
└── View/ (MVPのView)
```

## 2-2). Installation (導入手順)

### Installation (Runtime)

- ランタイムを導入する方法です
- Package/manifest.jsonに以下を追記します。
- (サンプルの導入手順は後述しています)

```json
{
  "dependencies": {
    "jp.synesthesias.plateau.snap": "https://github.com/Project-PLATEAU/PLATEAU-SNAP-App.git?path=SnapForUnity/Assets/Synesthesias.Snap",
    "com.cysharp.r3": "1.2.9",
    "com.cysharp.unitask": "2.5.10",
    "org.nuget.r3": "1.2.9",
    "com.google.ar.core.arfoundation.extensions": "https://github.com/google-ar/arcore-unity-extensions.git#1.47.0",
    "com.ishape.geometry": "https://github.com/iShapeUnity/Geometry.git#0.0.5",
    "com.ishape.mesh2d": "https://github.com/iShapeUnity/Mesh2d.git#0.0.9",
    "com.ishape.triangulation": "https://github.com/iShapeUnity/Triangulation.git#0.0.8"
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

### Sample (サンプル)

PackageManagerからSampleを追加する場合は上記のRuntime側のInstallationの手順に加えて更にPackage/manifest.jsonに以下を追記します。

```json
{
  "dependencies": {
    "com.unity.addressables": "1.22.3",
    "com.unity.localization": "1.5.4",
    "com.unity.textmeshpro": "3.0.6",
    "jp.hadashikick.vcontainer": "https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer#1.16.8",
    "org.nuget.polly": "8.5.2"
  }
}
```

## 2-3). Localizationの設定方法

- サンプルは `Localization` を使っています
- 初期設定を行なっていない場合は以下の手順で設定を行います
  - Edit > Project Settings > `Localization` (左側の一覧) > `Create` を選択
  - `Add Locale` を選択
  - 検索バーに `Japanese` と入力して検索してチェックを入れます (`Code` が `ja` のもの)
  - 同じように `English` (`Code` が `en` のもの)にチェックを入れます
  - `Add Locales` を選択します

## 2-4). Addressablesの設定方法

- サンプルは `Addressables` を使っています
- 初期設定を行なっていない場合は以下の手順で設定を行います
- Addressablesの初期設定を行なっていな場合
  - Window > Asset Management > Addressables > Groups > Create Addressables Settings > Create Addressables Settings
- 前述でインポートしたサンプルの `Addressables` ディレクトリを Project Viewから選択して Inspector の `Addressables` にチェックを入れます
  - 必要に応じてGroupを変更します
  - 全てのリソースをアプリに含める場合は `Default Local Group` のままにします

> Assets/Samples/Synesthesias PLATEAU SNAP/<バージョン>/Sample App/Addressables

## 2-5). 環境設定について

開発環境の設定は以下のScriptableObjectで管理しています。

- EnvironmentDevelopment (開発環境)

> Assets/Samples/Synesthesias.Snap/Resources/Environment/EnvironmentDevelopment.asset

- EnvironmentRelease (リリース環境)

> Assets/Samples/Synesthesias.Snap/Resources/Environment/EnvironmentRelease.asset

### 環境設定を追加する方法

- Project Viewで右クリックします
- Create > Synesthesias > Snap > Sample > EnvironmentScriptableObject

### 環境を切り替える方法

- Project ViewからRootLifetimeScopeのprefabを選択し  `Environment Scriptable Object` のフィールドに前述の環境設定のScriptableObjectの参照をドラッグ&ドロップで設定します
- デフォルトで `Environment Development` (開発環境) を指定済みです

> Assets/Samples/Synesthesias.Snap/Resources/VContainer/RootLifetimeScope.prefab

## 2-6). ARCoreの設定方法 (iOS)

- Unityプロジェクトを開きます
- Edit > Project Settings
- XR Plug-in Management > iOSタブを選択します
  - `Apple ARKit` にチェックを入れます
- XR Plug-in Management > ARCore Extensions
  - iOS Support Enabledにチェックを入れます
  - Geospatialにチェックを入れます

# ARCoreのAPIキーの管理に注意してください

- APIキーはgitで管理しないようにしください
- APIキーをアプリに組み込まないようにしてください
- APIキーはあくまで暫定対応です
  - Android: Keylessを使用してください
  - iOS: API Keyを選択します
- APIキーを間違ってコミットしないように以下の設定ファイルは.gitignoreで除外しています

```
/ProjectSettings/ARCoreExtensionsProjectSettings.json
```

### ARCoreのAPIキーの設定方法

- 以下のドキュメントの手順に従いARCoreのAPIキー作成の手順まで完了させます
  - https://developers.google.com/ar/develop/authorization?hl=ja&platform=unity-arf#api-key-unity
- Project Settings > XR Plugin-in Management > ARCore Extensions
- iOS Authentication StrategyをAPI Keyに設定します
  - 本来であればAuthentication Tokenを使用することを推奨します
  - iOS API Keyに先ほど発行したAPIキーを設定します

## 2-7). サーバーのAPI

- クライアント(Unity)は以下のサーバー側のリポジトリのAPIを呼んでいます
  - https://github.com/Project-PLATEAU/PLATEAU-SNAP-Server
- APIを呼ぶにはサーバー側で作成したAPIキーを後述の手順で設定する必要があります

# サーバーのAPIキーの管理に注意してください

- APIキーはgitで管理しないようにしください
- APIキーをアプリに組み込まないようにしてください
- APIキーはあくまで暫定対応です

### .gitignore用のディレクトリの作成

- gitでバージョン管理しないディレクトリを作成します
  - 例: Assets/Resources/GitIgnore
- .gitignoreに上記のディレクトリを追記します
- (本リポジトリのメンテナーは以下のディレクトリが既に.gitignoreに追記されているので、そのまま使ってください)

```
Assets/Resources/GitIgnore
```

### サーバーのAPIキーの設定方法

- `Assets/Resources/GitIgnore` ディレクトリで右クリックしてメニューを開きます
- Create > Synesthesias > Snap > Sample > ApiKeyScriptableObject を選択します
- 作成されたScriptableObjectを選択してAPIの情報を入力します
  - End Point
    - サーバーのエンドポイントのURLを入力します
  - Api Key Type
    - APIキーの種類を入力します
    - 例: Bearer, X-API-Key
  - Api Key Value
    - APIキーの値を入力します
- Project Viewから前述の開発環境設定用のScriptableObjectを選択し `Api Configuration` のフィールドに前述のScriptableObjectの参照をドラッグ&ドロップで設定します

## 2-8). iOS機能用途の説明の記載

本アプリはiOSのカメラと位置情報の機能を使用するため、用途を記載しないとビルドエラーが発生してしまいます。

- Edit > Project Settings > Player Settings (左側のメニューのPlayer)
- iOSタブを選択
- `Camera Usage Description` にカメラの用途を追記します
  - 例: `建物検出機能に使用します`
- `Location Usage Description` に位置情報の用途を追記します
  - 例: `建物検出機能に使用します`

## 2-9). サンプルアプリのビルド方法

- サンプルをインポートして前述のAPIキーの設定を一通り完了させます
- File > Build Settings
- `iOS` を選択して `Switch Platform` を選択します
- 以下のシーンを順番に開いて Build Settingsの `Add Open Scenes` を選択して各種シーンを `Scenes In Build` に追加します
  - BootScene (必ず0番目にしてください)
  - MobileDetectionScene
  - GuideScene
  - MainScene
  - ValidationScene
- `Run in Xcode as` は `Release` が選択されていることを確認します
- `Development Build` のチェックが外れていることを確認します
  - ＊ `Development Build` の場合はアプリが起動できません
  - (Development Buildでもアプリが実行する方法があれば情報共有をお願いします)
- `Build` を選択してiOSのアプリをビルドします

## 2-10). Xcodeのビルド方法

以下は必要最低限アプリをビルドするまでの手順です。
Xcodeの詳細な使い方については触れないため、必要に応じて適宜調査をお願いします。

- MacにiPhone端末を有線で事前に接続しておきます
- ドロップダウンから接続しているiPhoneを選択しておきます
- 初回ビルドの場合Signingでビルドエラーになるのでい以下のいずれかを設定します
  - Automatically manage signing
  - Provisioning Profile
- 再生アイコンを押すと再度ビルドが開始されます
- ビルドが完了するとiPhoneにアプリが自動でインストールされアプリが起動します

## 2-12). サンプルアプリを使用する

サンプルアプリの使い方は以下のページからご確認ください。

- [アプリ利用者向け情報](../user/index.md)

## 2-13). APIドキュメントの閲覧方法

- 本ドキュメントの画面上部の「API」タブを選択してご確認ください。
