# 開発者(Developer)向け情報

本アプリの Runtime と Sample(後述)を利用してアプリの開発を行う開発者(Developer)向けの情報です。

本ページでは、サンプルアプリをビルド・実行するまでの流れをまとめています。

### 必要なデバイス
デバイスとして、iPhoneとMacが必要です。
下記に動作確認済みの環境を示します。

| 種別 | デバイス | 機種名 | OS |
| --- | --- | --- | --- | 
| 実機 (実行用) | iPhone | iPhone 13 Pro | iOS 18.3 | 
| ビルド環境 | Mac | MacBook Pro (M1) | macOS Sonoma 14.7 | 

> [!NOTE]
> - iPhone は ARKit / ARCore 対応の機種が必要です
> - Mac は Xcode がインストールされている必要があります

---

## 構成 (Architecture)

本アプリは Unity(開発プラットフォーム)で開発しています。
以下の構成になっています。

-   [Runtime](https://github.com/Project-PLATEAU/PLATEAU-SNAP-App/tree/main/SnapForUnity/Assets/Synesthesias.Snap/Runtime) (ランタイム)
    -   アプリ開発に必要なランタイムが含まれています
    -   アーキテクチャは MVP (Model View Presenter)を採用していますが Presenter は含まれていません
-   [Sample](https://github.com/Project-PLATEAU/PLATEAU-SNAP-App/tree/main/SnapForUnity/Assets/Synesthesias.Snap/Samples~/Scripts) (サンプル)
    -   UI、シーン等のリソースを含むサンプルアプリです
    -   前述のランタイムが必要です
    -   アーキテクチャは MVP (Model View Presenter)を採用しています

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

### Runtime (ランタイム)

**Version (バージョン)**

[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=PLATEAU.SNAP.App&message=0.0.1&color=0e6da0)](https://github.com/Project-PLATEAU/PLATEAU-SNAP-App)

**Support (サポート対象)**

[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Unity&message=2022.3.44f1%20or%20higher&color=0e6da0)](https://github.com/Gentlymad-Studios/PackageManagerTools)

**Dependencies (依存パッケージ)**

[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=PLATEAU.SNAP.Server&message=0.0.1&color=0e6da0)](https://github.com/Project-PLATEAU/PLATEAU-SNAP-Server)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=AR%20Foundation&message=5.1.5&color=0e6da0)](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@5.1/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Google%20ARCore%20XR%20Plugin&message=5.1.5&color=0e6da0)](https://docs.unity3d.com/ja/Packages/com.unity.xr.arkit@5.1/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=ARCore%20Extensions&message=1.22.3&color=0e6da0)](https://github.com/google-ar/arcore-unity-extensions)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=UniTask&message=2.5.10&color=0e6da0)](https://github.com/Cysharp/UniTask/releases/tag/2.5.10)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=R3&message=1.2.9&color=0e6da0)](https://github.com/Cysharp/R3/releases/tag/1.2.9)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=Geometry&message=0.0.5&color=0e6da0)](https://github.com/iShapeUnity/Geometry/releases/tag/0.0.5)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=Mesh2d&message=0.0.9&color=0e6da0)](https://github.com/iShapeUnity/Mesh2d/releases/tag/0.0.9)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=Triangulation&message=0.0.8&color=0e6da0)](https://github.com/iShapeUnity/Triangulation/releases/tag/0.0.8)

### Sample (サンプル)

**Dependencies (依存パッケージ)**

[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Addressables&message=1.22.3&color=0e6da0)](https://docs.unity3d.com/Packages/com.unity.addressables@1.22/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Localization&message=1.5.4&color=0e6da0)](https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=TextMeshPro&message=3.0.6&color=0e6da0)](https://docs.unity3d.com/ja/2022.3/Manual/com.unity.textmeshpro.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=VContainer&message=1.16.8&color=0e6da0)](https://github.com/hadashiA/VContainer/releases/tag/1.16.8)
[![](https://img.shields.io/static/v1?style=flat=square&logo=NuGet&logoColor=FFFFFF&label=Polly&message=8.5.2&color=0e6da0)](https://www.nuget.org/packages/Polly/8.5.2)

<br>

サーバとの通信 API も含めたコードの詳細仕様は、本ドキュメントの画面上部の「API」タブを選択してご確認ください。

---

## 環境設定について

#### 環境設定の概要

サンプルアプリではアプリの動作環境として、開発環境・リリース環境を用意しており、以下の ScriptableObject で管理しています。

-   EnvironmentDevelopment (開発環境)

> Assets/Samples/Synesthesias.Snap/Resources/Environment/EnvironmentDevelopment.asset

-   EnvironmentRelease (リリース環境)

> Assets/Samples/Synesthesias.Snap/Resources/Environment/EnvironmentRelease.asset

#### 環境の設定

使用する環境を切り替えるには、以下の手順で参照を設定します。

-   Project View から RootLifetimeScope の prefab を選択し `Environment Scriptable Object` のフィールドに前述の環境設定の ScriptableObject の参照をドラッグ&ドロップで設定します
-   デフォルトで `Environment Development` (開発環境) を指定済みです

> Assets/Samples/Synesthesias.Snap/Resources/VContainer/RootLifetimeScope.prefab

> [!NOTE]
> 環境を追加する方法
> -   Project View で右クリックします
> -   Create > Synesthesias > Snap > Sample > EnvironmentScriptableObject

---

## ARCore の設定方法 (iOS)

本アプリは iOS の AR 機能を使用するため、Unity プロジェクト側で ARCore 関連の設定が必要です。

#### プラグインの設定

まず、Unity のプロジェクト設定で ARCore 関連の機能を有効化します。

-   Unity プロジェクトを開きます
-   Edit > Project Settings
-   XR Plug-in Management > iOS タブを選択します
    -   `Apple ARKit` にチェックを入れます
-   XR Plug-in Management > ARCore Extensions
    -   iOS Support Enabled にチェックを入れます
    -   Geospatial にチェックを入れます

#### ARCore の API キーの設定方法

次に、Google Cloud Platform で発行した API キーをプロジェクトに設定します。

-   以下のドキュメントの手順に従い ARCore の API キー作成の手順まで完了させます
    -   https://developers.google.com/ar/develop/authorization?hl=ja&platform=unity-arf#api-key-unity
-   Project Settings > XR Plugin-in Management > ARCore Extensions
-   iOS Authentication Strategy を API Key に設定します
    -   本来であれば Authentication Token を使用することを推奨します
    -   iOS API Key に先ほど発行した API キーを設定します

> [!Important]
> 【重要】ARCore の API キーの管理について
> API キーの取り扱いには十分注意してください。
> -   API キーは git で管理しないようにしてください
> -   API キーをアプリに組み込まないようにしてください
> -   API キーはあくまで暫定対応です
>     -   Android: Keyless を使用してください
>     -   iOS: API Key を選択します
> -   API キーを間違ってコミットしないように以下の設定ファイルは.gitignore で除外しています
> ```
> /ProjectSettings/ARCoreExtensionsProjectSettings.json
> ```

---

## サーバーの API 設定

本アプリはサーバーと通信を行うため、API キーの設定が必要です。
以下の手順に従って設定を行ってください。

#### サーバーリポジトリについて

クライアント(Unity)は以下のサーバー側の API を利用しています。

-   https://github.com/Project-PLATEAU/PLATEAU-SNAP-Server


#### 保存用ディレクトリの準備

API キーを含む設定ファイルを保存するための、Git 管理外のディレクトリを用意します。

-   `Assets/Resources/GitIgnore` ディレクトリが存在することを確認します（なければ作成します）。
-   念のため `.gitignore` に以下の記述があるか確認します（本リポジトリでは設定済みです）。

```
Assets/Resources/GitIgnore
```

#### API キー設定アセットの作成

保存用ディレクトリの中に、API キー情報を保持する設定ファイル（ScriptableObject）を作成します。

-   `Assets/Resources/GitIgnore` ディレクトリで右クリックし、メニューを開きます
-   Create > Synesthesias > Snap > Sample > ApiKeyScriptableObject を選択します
-   作成されたアセットを選択し、Inspector で以下を入力します
    -   **End Point**: サーバーのエンドポイント URL
    -   **Api Key Type**: キーの種類（例: `Bearer`, `X-API-Key`）
    -   **Api Key Value**: API キーの値

#### 環境設定への適用

作成した API キー設定を、アプリの環境設定（EnvironmentScriptableObject）に紐付けます。

-   使用する環境設定アセット（例: `EnvironmentDevelopment.asset`）を選択します
    -   場所: `Assets/Samples/Synesthesias.Snap/Resources/Environment/`
-   Inspector の `Api Configuration` フィールドに、手順 2 で作成した `ApiKeyScriptableObject` をドラッグ&ドロップします


> [!Important]
> 【重要】API キーの取り扱いについて
> セキュリティ上の理由から、API キーは以下のように管理してください。
> -   Git で管理しない（.gitignore で除外されたフォルダに保存する）
> -   アプリ本体のコードに直接埋め込まない

---

## iOS 機能用途の説明の記載

本アプリは iOS のカメラと位置情報の機能を使用するため、申請時やビルド時に用途の説明が必要です。
**サンプルアプリでは既に設定が含まれていますので、設定が正しいか確認してください。**

#### Player Settings の確認

-   Edit > Project Settings > Player Settings (左側のメニューの Player)
-   iOS タブを選択
-   `Camera Usage Description` にカメラの用途が記載されているか確認します
    -   例: `建物検出機能に使用します`
-   `Location Usage Description` に位置情報の用途が記載されているか確認します
    -   例: `建物検出機能に使用します`

---

## iPhoneへのビルド

ここまでの設定が完了したら、以下の 2 ステップでアプリをiPhoneにインストールします。

### Step 1: Unity から Xcode プロジェクトを書き出す

Unity から iOS アプリのプロジェクトを出力します。

**Build Settings の設定**

-   サンプルをインポートして前述の API キーの設定を一通り完了させます
-   File > Build Settings
-   `iOS` を選択して `Switch Platform` を選択します
-   以下のシーンを順番に開いて Build Settings の `Add Open Scenes` を選択して各種シーンを `Scenes In Build` に追加します
    -   BootScene (必ず 0 番目にしてください)
    -   MobileDetectionScene
    -   GuideScene
    -   MainScene
    -   ValidationScene
-   `Run in Xcode as` は `Release` が選択されていることを確認します
-   `Development Build` のチェックが外れていることを確認します
    -   ＊ `Development Build` の場合はアプリが起動できません
    -   (Development Build でもアプリが実行する方法があれば情報共有をお願いします)
-   `Build` を選択して iOS のアプリをビルドします

### Step 2: Xcode でのビルド

Unity でビルドしたプロジェクトを Xcode で開き、iPhoneにインストールします。
以下は必要最低限アプリをビルドするまでの手順です。
Xcode の詳細な使い方については触れないため、必要に応じて適宜調査をお願いします。

-   Mac に iPhone 端末を有線で事前に接続しておきます
-   ドロップダウンから接続している iPhone を選択しておきます
-   初回ビルドの場合 Signing でビルドエラーになるので以下のいずれかを設定します
    -   Automatically manage signing
    -   Provisioning Profile
-   再生アイコンを押すと再度ビルドが開始されます
-   ビルドが完了すると iPhone にアプリが自動でインストールされアプリが起動します

---

## サンプルアプリを使用する

インストールされたサンプルアプリの具体的な使い方は、利用者向けドキュメントを参照してください。

-   [アプリ利用者向け情報](../user/index.md)

> [!NOTE]
> 上記以外にもテストフライトやAdHocなどの配信方法があります。

<br>
<br>

---

