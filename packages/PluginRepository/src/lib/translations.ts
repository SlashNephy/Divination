// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable @typescript-eslint/naming-convention */

export type Translation = {
  TopDescription: string
  HowToUseHeader: string
  HowToUseDescription: string
  DisclaimerHeader: string
  DisclaimerDescription: string
  PluginListHeader: string
  PluginListDescription: string
  PluginListViewSourceCodeLabel: string
  PluginListDownloadStableButton: string
  PluginListDownloadTestingButton: string
}

export const english: Translation = {
  TopDescription:
    '<0>xiv.starry.blue</0> is a third-party Dalamud plugin repository that can be used with XIVLauncher.',
  HowToUseHeader: 'How to Use',
  HowToUseDescription: 'Use the following URL. Drop the URL into your third-party repository list.',
  DisclaimerHeader: 'Disclaimer',
  DisclaimerDescription:
    'Please install plugins distributed in this repository at your own risk. We do not take responsibility for any damages caused by the use of our plugins.',

  PluginListHeader: 'Plugin List',
  PluginListDescription: 'Here is a list of available plugins.',
  PluginListViewSourceCodeLabel: 'View source code',
  PluginListDownloadStableButton: 'Download Stable',
  PluginListDownloadTestingButton: 'Download Testing',
}

export const japanese: Translation = {
  TopDescription: '<0>xiv.starry.blue</0> は XivLauncher 用の Dalamud プラグインリポジトリです。',
  HowToUseHeader: '使い方',
  HowToUseDescription: 'XIVLauncher で使用できるようにするには、以下のURLをリポジトリ リストに追加してください。',
  DisclaimerHeader: '免責事項',
  DisclaimerDescription:
    'このリポジトリで配布しているプラグインは自己責任で使用してください。私たちはこのリポジトリに含まれるプラグインの使用によって発生した損害に対して、一切の責任を負いません。',
  PluginListHeader: 'プラグイン一覧',
  PluginListDescription: 'このリポジトリで配布しているプラグインの一覧です。',
  PluginListViewSourceCodeLabel: 'ソースコードを GitHub で表示',
  PluginListDownloadStableButton: '安定版をダウンロード',
  PluginListDownloadTestingButton: 'テスト版をダウンロード',
}
