# PLATEAU-SNAP-App
デジタルツインの実現に向けたクラウドソーシング型3D都市モデル作成システム(PLATEAU SNAP)のモバイルアプリケーション

## Runtime

### Version

[![](https://img.shields.io/static/v1?style=flat=square&logo=GitHub&logoColor=FFFFFF&label=PLATEAU&nbsp;SNAP&message=0.0.1&color=0e6da0)](https://github.com/Gentlymad-Studios/PackageManagerTools)

### Support

[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Unity&message=2022.3.44f1%20or%20higher&color=0e6da0)](https://github.com/Gentlymad-Studios/PackageManagerTools)

### Dependencies

[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=Addressables&message=1.22.3&color=0e6da0)](https://docs.unity3d.com/Packages/com.unity.addressables@1.22/manual/index.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=TextMeshPro&message=3.0.6&color=0e6da0)](https://docs.unity3d.com/ja/2022.3/Manual/com.unity.textmeshpro.html)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=UniTask&message=2.5.10&color=0e6da0)](https://github.com/Cysharp/UniTask/releases/tag/2.5.10)
[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=R3&message=1.2.9&color=0e6da0)](https://github.com/Cysharp/R3/releases/tag/1.2.9)

### Installation

Package/manifest.jsonに以下を追記します。
(サンプルの場合は後述)

```json
{
  "dependencies": {
    "com.cysharp.r3": "1.2.9",
    "com.cysharp.unitask": "2.5.10",
    "com.unity.addressables": "1.22.3",
    "com.unity.textmeshpro": "3.0.6",
    "org.nuget.r3": "1.2.9"
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

[![](https://img.shields.io/static/v1?style=flat=square&logo=Unity&logoColor=FFFFFF&label=VContainer&message=1.16.8&color=0e6da0)](https://github.com/hadashiA/VContainer/releases/tag/1.16.8)

### Installation

PackageManagerからSampleを追加する場合は上記のRuntime側のInstallationの対応に加えて更にPackage/manifest.jsonに以下を追記します。

```json
{
  "dependencies": {
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
