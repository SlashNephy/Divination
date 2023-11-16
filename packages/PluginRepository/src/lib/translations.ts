export type Translation = {
  TopDescription: string
  HowToUseHeader: string
  HowToUseDescription: string
  DisclaimerHeader: string
  DisclaimerDescription: string
  PluginListHeader: string
  PluginListDescription: string
  PluginListShowTestingVersions: string
  PluginListTableHeaderName: string
  PluginListTableHeaderDescription: string
  PluginListTableHeaderVersion: string
  PluginListTableHeaderDownloads: string
}

export const english: Translation = {
  TopDescription:
    '<0>xiv.starry.blue</0> is a third-party Dalamud plugin repository that can be used with XIVLauncher.',
  HowToUseHeader: 'How to Use',
  HowToUseDescription: 'Use the following URL. Drop the URL into your third-party repository list.',
  DisclaimerHeader: 'Disclaimer',
  // eslint-disable-next-line xss/no-mixed-html
  DisclaimerDescription:
    'Please install plugins distributed in this repository at your own risk.<br />We do not take responsibility for any damages caused by the use of our plugins.',
  PluginListHeader: 'Plugin List',
  PluginListDescription: 'Here is a list of available plugins.',
  PluginListShowTestingVersions: 'Show Testing Versions',
  PluginListTableHeaderName: 'NAME',
  PluginListTableHeaderDescription: 'DESCRIPTION',
  PluginListTableHeaderVersion: 'VERSION',
  PluginListTableHeaderDownloads: 'DOWNLOADS',
}

export const japanese: Translation = {
  TopDescription: '<0>xiv.starry.blue</0> は XivLauncher 用の Dalamud プラグインリポジトリです。',
  HowToUseHeader: '使い方',
  HowToUseDescription: 'XIVLauncher で使用できるようにするには、以下のURLをリポジトリ リストに追加してください。',
  DisclaimerHeader: '免責事項',
  // eslint-disable-next-line xss/no-mixed-html
  DisclaimerDescription:
    'このリポジトリで配布しているプラグインは自己責任で使用してください。<br />私たちは、このリポジトリに含まれるプラグインの使用によって発生した損害に対して、一切の責任を負いません。',
  PluginListHeader: 'プラグイン一覧',
  PluginListDescription: 'このリポジトリで配布しているプラグインの一覧です。',
  PluginListShowTestingVersions: '開発中のバージョンを表示する',
  PluginListTableHeaderName: '名前',
  PluginListTableHeaderDescription: '概要',
  PluginListTableHeaderVersion: 'バージョン',
  PluginListTableHeaderDownloads: 'ダウンロード数',
}
