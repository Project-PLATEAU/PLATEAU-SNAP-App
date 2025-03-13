# 3). コントリビューター(Contributer)向け情報

- 本アプリのコントリビューター(Contributer)向け情報です
- 本リポジトリに対して実際にコミットやプルリクエストを作成する方はこちらのページのご確認をお願いいたします
- [Wiki](https://github.com/Synesthesias/PLATEAU-SNAP-App/wiki)の「開発者向け情報」も合わせてご確認ください

## 3-1). 開発環境

- 本アプリはiOSを含むモバイルデバイスがサポート対象です
- 上記のため本ページはMacユーザーの開発者をベースとした情報が前提となっております
- UnityはWindowsでも開発が可能なためコマンド等はWindows版に置き換えて読み進めて下さい

## 3-2). サンプルを更新する場合の注意点

Unityパッケージの仕様により、サンプルファイルは Samples~ ディレクトリ(隠しディレクトリ)に配置されています。この仕様は [Unity公式ドキュメント](https://docs.unity3d.com/ja/2022.3/Manual/cus-samples.html)に従ったもので、Samples ディレクトリにリネームする代わりに、シンボリックリンクを活用して開発の効率化を図っています。

### Windowsユーザー向けシンボリックリンクの設定手順

Windowsユーザーの方はリポジトリ内で以下のコマンドを実行し、シンボリックリンクを有効化してください。

```bash
git config core.symlinks true
```

Macユーザーは特別な対応は不要です。

## 3-3). クライアントのAPIドキュメントの作成方法(追加対応不要)

- 既に初期設定は行なっているため追加対応は不要です
- 以下の手順で各種設定ファイルは作成しています

```bash
// .NETのインストール
brew install dotnet

// リポジトリのルートディレクトリへ移動
cd path/to/PLATEAU-SNAP-App

// .NET環境を作成(.config/dotnet-tools.jsonが作成される)
dotnet new tool-manifest

// .NET経由でDocFxを導入(dotnet-tools.jsonにdocfxが追記される)
dotnet tool install docfx

// DocFxのバージョンを調整(下記参照)
vi .config/dotnet-tools.json

// バージョンを変更したので.NET環境を復元
dotnet tool restore

// DocFxの初期化(質問に順番に答えるとdocsディレクトリが生成される)
dotnet docfx init -o docs

// APIドキュメントに含めるcsprojファイルを指定(下記参照)
vi docs/docfx.json
```

### DocFxのバージョンについて

最新のバージョンだとAPIドキュメント生成時にエラーになってしまうためバージョン `2.78.2` から `2.75.3` にダウングレードしています。

### DocFxの初期化時の質問項目(追加対応不要)

```bash
// サイトの名称
Name (mysite): Synesthesias PLATEAU SNAP ドキュメント

// APIドキュメントを作成するかどうか
Generate .NET API documentation? [y/n] (y): ※そのままエンターキーを押す

// Unityプロジェクトディレクトリを指定(csprojファイルのパス)
.NET projects location (src): ./SnapForUnity

// ドキュメントのディレクトリ
Markdown docs location (docs): ※そのままエンターキーを押す

// 検索バーが表示するかどうか
Enable site search? [y/n] (y): ※そのままエンターキーを押す

// PDFのダウンロードリンクを表示するかどうか
Enable PDF? [y/n] (y): ※そのままエンターキーを押す

// 設定の最終確認
Is this OK? [y/n] (y): ※そのままエンターキーを押す
```

### APIドキュメントに含めるcsprojを指定

> docs/[docfx.json](https://github.com/Synesthesias/PLATEAU-SNAP-App/blob/main/docs/docfx.json)

```diff
{
  "$schema": "https://raw.githubusercontent.com/dotnet/docfx/main/schemas/docfx.schema.json",
  "metadata": [
    {
      "src": [
        {
          "src": ".././SnapForUnity",
          "files": [
-           "**/*.csproj"
+           "Synesthesias.Snap.Generated.Sample.csproj",
+           "Synesthesias.Snap.csproj",
+           "Synesthesias.Snap.Sample.csproj"
          ]
        }
      ],
      "dest": "api"
+     "allowCompilationErrors": true
    }
  ]
}
```

## 3-4). クライアントのAPIドキュメントの更新方法

### 更新用ブランチへ切替

gitでmainブランチを最新の状態にしてdocs更新用のブランチへ切り替えます

```bash
// リモートの最新情報を取得
git fetch origin

// mainブランチを最新の状態に同期する
git branch -f main origin/main

// mainブランチからdocs更新用のブランチを作成してチェックアウト
git checkout -b feature/update_docs main
```

### csprojファイルの生成

APIドキュメントは `*.csproj` ファイルを元に生成されます。

Unityで該当のUnityプロジェクトを開き直したり、ソースコードを任意のIDEで開きます。

Unityプロジェクトのディレクトリ直下に `*.csproj` ファイルが生成されていることを確認します。

### ドキュメントの生成

```bash
// .NETのインストール
brew install dotnet

// リポジトリのルートディレクトリへ移動
cd path/to/PLATEAU-SNAP-App

// .NET環境を復元
dotnet tool restore

// 既存ファイルを削除
rm -rf docs/api docs/_site docs/api docs/docs

// ymlファイル作成
dotnet docfx metadata docs/docfx.json
```

### ドキュメントの確認

以下のコマンドを実行するとローカル環境でDocFxで生成したドキュメントをブラウザで閲覧することが可能です。

```bash
// htmlファイル作成(docs/_site)
dotnet docfx build docs/docfx.json

// ホスティングとpdfドキュメント生成(前述のコマンドをスキップしてこちらを直接実行してもymlとhtmlファイルは作成されます)
dotnet docfx docs/docfx.json --serve --port <ポート番号>

// ブラウザから以下のURLを開く
http://localhost:<ポート番号>
```

### PRマージと自動デプロイ

- ここまでの変更差分をpushしてプルリクエストをmainブランチへマージします
- mainブランチに変更が加わると[GitHub Action](https://github.com/Synesthesias/PLATEAU-SNAP-App/blob/main/.github/workflows/deploy-docs.yml)が自動で `docs/_site` 配下のファイルを [gh-pages](https://github.com/Synesthesias/PLATEAU-SNAP-App/tree/gh-pages) ブランチへ自動でpushします
- 自動で[ドキュメント](https://synesthesias.github.io/PLATEAU-SNAP-App)ページにデプロイされ更新されます(詳細後述)

#### 自動デプロイの設定について

前述のデプロイはリポジトリの `Settings` タブで設定されています
`Settings` タブへのアクセスはアクセス権限が必要です。

- [詳細](https://docs.github.com/ja/pages/getting-started-with-github-pages/creating-a-github-pages-site)
- 本リポジトリは以下の設定になっているため `gh-pages` ブランチへ `_site` 配下のディレクトリがpushされると自動でドキュメントページへ自動デプロイが行われます

> Build and deployment
> Source: Deploy from a branch
> Branch: gh-pages

## 3-5). サンプルのAPI通信用のクライアントコードの更新方法

- [サーバー側](https://github.com/Synesthesias/PLATEAU-SNAP-Server)のAPIが更新されたら [spec.json](https://github.com/Synesthesias/PLATEAU-SNAP-App/blob/main/OpenAPI/spec.json) を更新してください
- 以下の手順を参考にOpenAPIのGeneratorでGenerated.Clientディレクトリ配下のソースコードを更新します

```bash
// openapi-generatorのインストール
brew install openapi-generator

// OpenAPIのディレクトリへ移動
cd path/to/OpenAPI

// 既存の生成コードの削除
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

## 3-6). プルリクエストの作成手順

- Githubのリポジトリの画面上部から `Pull requests` タブを選択します
- pushしたブランチの `Compare & Pull request` または `New pull request` からブランチを選択してから `Create pull request` を選択してください

### 機能実装のプルリクエストの場合

- 表示されたテンプレートをそのまま使ってください
- (またはURLの末尾に `?template=feature_template.md` を追記してください)
- テンプレートを更新する場合は[こちら](https://github.com/Synesthesias/PLATEAU-SNAP-App/blob/main/.github/pull_request_template.md)のファイルを更新して下さい

### 不具合修正のプルリクエストの場合

- URLの末尾に `?template=fix_template.md` を追記してください
- テンプレートを更新する場合は[こちら](https://github.com/Synesthesias/PLATEAU-SNAP-App/blob/main/.github/PULL_REQUEST_TEMPLATE/fix_template.md)のファイルを更新して下さい

### その他のプルリクエストの場合

- テンプレートは作成していないです
- 必要に応じて[PULL_REQUEST_TEMPLATE](https://github.com/Synesthesias/PLATEAU-SNAP-App/tree/main/.github/PULL_REQUEST_TEMPLATE)ディレクトリ配下に追加してください
