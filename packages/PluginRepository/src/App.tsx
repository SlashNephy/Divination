import { Badge, Card, Checkbox, Col, Container, Link, Loading, Row, Spacer, Table, Text } from '@nextui-org/react'
import { IconAlertTriangleFilled, IconBook, IconPuzzle } from '@tabler/icons-react'
import React, { Suspense, useState } from 'react'
import { Trans, useTranslation } from 'react-i18next'

import { usePluginMaster } from './lib/usePluginMaster.ts'

export function App(): React.JSX.Element {
  const { t } = useTranslation()
  const [isTesting, setIsTesting] = useState(false)

  return (
    <Container>
      <Card css={{ pl: '$6' }}>
        <Card.Body>
          <Row>
            <Text>
              <Trans i18nKey="TopDescription">
                <Link href="/" />
              </Trans>
            </Text>
          </Row>
        </Card.Body>
      </Card>

      <Spacer y={1} />

      <Card css={{ pl: '$6', pr: '$6' }}>
        <Card.Body>
          <Col>
            <Text h2>
              <IconBook />️ {t('HowToUseHeader')}
            </Text>
            <Text>{t('HowToUseDescription')}</Text>
            <Text blockquote>https://xiv.starry.blue/plugins/master.json</Text>
          </Col>

          <Col>
            <Text h2>
              <IconAlertTriangleFilled />️ {t('DisclaimerHeader')}
            </Text>
            <Text>
              <Trans i18nKey="DisclaimerDescription" />
            </Text>
          </Col>
        </Card.Body>
      </Card>

      <Spacer y={1} />

      <Card css={{ pl: '$6' }}>
        <Card.Body>
          <Col>
            <Text h2>
              <IconPuzzle />️ {t('PluginListHeader')}
            </Text>
            <Text>{t('PluginListDescription')}</Text>
            <Checkbox isSelected={isTesting} size="sm" onChange={setIsTesting}>
              {t('PluginListShowTestingVersions')}
            </Checkbox>

            <Suspense
              fallback={
                <Row justify="center">
                  <Loading />
                </Row>
              }
            >
              <PluginList isTesting={isTesting} />
            </Suspense>
          </Col>
        </Card.Body>
      </Card>
    </Container>
  )
}

type PluginListProps = {
  isTesting: boolean
}

export function PluginList({ isTesting }: PluginListProps): React.JSX.Element {
  const { t } = useTranslation()
  const plugins = usePluginMaster()

  return (
    <Table>
      <Table.Header>
        <Table.Column>{t('PluginListTableHeaderName')}</Table.Column>
        <Table.Column>{t('PluginListTableHeaderDescription')}</Table.Column>
        <Table.Column>{t('PluginListTableHeaderVersion')}</Table.Column>
        <Table.Column>{t('PluginListTableHeaderDownloads')}</Table.Column>
      </Table.Header>
      <Table.Body>
        {plugins
          .sort((a, b) => b.DownloadCount - a.DownloadCount)
          .map((plugin) => (
            <Table.Row key={plugin.InternalName}>
              <Table.Cell>
                <Link
                  href={`https://github.com/SlashNephy/Divination/tree/master/packages/Plugins/${plugin.InternalName}`}
                  target="_blank"
                >
                  {plugin.InternalName}
                </Link>
              </Table.Cell>
              <Table.Cell>
                {plugin.Description && (
                  <>
                    <p style={{ wordWrap: 'break-word', wordBreak: 'break-all', width: '100%', tableLayout: 'fixed' }}>
                      {plugin.Description}
                    </p>
                    <br />
                  </>
                )}
                {plugin.CategoryTags.map((tag) => (
                  <Badge key={tag} color="secondary" variant="flat">
                    {tag.charAt(0).toUpperCase() + tag.slice(1)}
                  </Badge>
                ))}
                {plugin.Tags.map((tag) => (
                  <Badge key={tag} color="primary" variant="flat">
                    #{tag}
                  </Badge>
                ))}
              </Table.Cell>
              <Table.Cell>
                <Link href={isTesting ? plugin.DownloadLinkTesting : plugin.DownloadLinkInstall} target="_blank">
                  {isTesting ? plugin.TestingAssemblyVersion : plugin.AssemblyVersion}
                </Link>{' '}
                (API {plugin.DalamudApiLevel})
              </Table.Cell>
              <Table.Cell>{plugin.DownloadCount}</Table.Cell>
            </Table.Row>
          ))}
      </Table.Body>
    </Table>
  )
}
