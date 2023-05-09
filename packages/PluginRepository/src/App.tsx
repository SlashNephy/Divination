import { Badge, Button, Card, Col, Container, Grid, Link, Loading, Row, Spacer, Text, Tooltip } from '@nextui-org/react'
import { IconAlertTriangleFilled, IconBook, IconBrandGithub, IconPuzzle } from '@tabler/icons-react'
import React, { Suspense } from 'react'
import { Trans, useTranslation } from 'react-i18next'

import { usePluginMaster } from './lib/usePluginMaster.ts'
import packageJson from '../package.json'

export function App(): React.JSX.Element {
  const { t } = useTranslation()

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
            <Text>{t('DisclaimerDescription')}</Text>
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

            <Suspense
              fallback={
                <Row justify="center">
                  <Loading />
                </Row>
              }
            >
              <PluginList />
            </Suspense>
          </Col>
        </Card.Body>
      </Card>
    </Container>
  )
}

export function PluginList(): React.JSX.Element {
  const { t } = useTranslation()
  const plugins = usePluginMaster()

  return (
    <Grid.Container gap={2}>
      {plugins
        .sort((a, b) => b.DownloadCount - a.DownloadCount)
        .map((plugin) => (
          <Grid key={plugin.InternalName} md={4}>
            <Card css={{ p: '$4' }}>
              <Card.Header>
                <Col>
                  <Text h4 css={{ lineHeight: '$xs' }}>
                    {plugin.InternalName} (API {plugin.DalamudApiLevel})
                  </Text>
                  <Text css={{ color: '$accents8' }}>{plugin.Punchline}</Text>
                </Col>
              </Card.Header>
              <Card.Body css={{ py: '$2' }}>
                <Row>
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
                  <Badge variant="flat">Downloads: {plugin.DownloadCount}</Badge>
                  <Badge variant="flat">Author: {plugin.Author}</Badge>
                </Row>

                <Text>{plugin.Description}</Text>

                <Link
                  href={`${packageJson.repository}/tree/master/packages/Plugins/${plugin.InternalName}`}
                  target="_blank"
                >
                  <IconBrandGithub />
                  {t('PluginListViewSourceCodeLabel')}
                </Link>
              </Card.Body>
              <Card.Footer>
                <Row justify="center">
                  {/* @ts-expect-error broken type Tooltip */}
                  <Tooltip rounded content={`v${plugin.AssemblyVersion}`}>
                    <Link href={plugin.DownloadLinkInstall} target="_blank">
                      <Button auto color="primary">
                        {t('PluginListDownloadStableButton')}
                      </Button>
                    </Link>
                  </Tooltip>

                  <Spacer x={1} />

                  {/* @ts-expect-error broken type Tooltip */}
                  <Tooltip rounded content={`v${plugin.TestingAssemblyVersion}`}>
                    <Link href={plugin.DownloadLinkTesting} target="_blank">
                      <Button auto color="error">
                        {t('PluginListDownloadTestingButton')}
                      </Button>
                    </Link>
                  </Tooltip>
                </Row>
              </Card.Footer>
            </Card>
          </Grid>
        ))}
    </Grid.Container>
  )
}
