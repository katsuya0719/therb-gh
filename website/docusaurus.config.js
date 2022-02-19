// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require('prism-react-renderer/themes/github');
const darkCodeTheme = require('prism-react-renderer/themes/dracula');

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'THERB 2.0',
  tagline: 'Renewal toolset for building thermal simulation',
  url: ' http://katsuya0719.github.io/therb-gh/',
  baseUrl: '/',
  onBrokenLinks: 'ignore',
  onBrokenMarkdownLinks: 'warn',
  favicon: 'img/becat_logo.jpg',
  organizationName: 'katsuya0719', // Usually your GitHub org/user name.
  projectName: 'therb-gh', // Usually your repo name.
  // i18n:{
  //   defaultLocale: 'ja',
  //   locales:['ja','en'],
  //   localeConfigs:{
  //     ja:{
  //       label:'日本語',
  //     },
  //     en:{
  //       label:'English',
  //     }
  //   }
  // },

  presets: [
    [
      'classic',
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          sidebarPath: require.resolve('./sidebars.js'),
          // Please change this to your repo.
          editUrl: 'https://github.com/facebook/docusaurus/tree/main/packages/create-docusaurus/templates/shared/',
        },
        blog: {
          showReadingTime: true,
          // Please change this to your repo.
          editUrl:
            'https://github.com/facebook/docusaurus/tree/main/packages/create-docusaurus/templates/shared/',
        },
        theme: {
          customCss: require.resolve('./src/css/custom.css'),
        },
      }),
    ],
  ],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      navbar: {
        title: 'THERB 2.0',
        logo: {
          alt: 'My Site Logo',
          src: 'img/becat_logo.jpg',
        },
        items: [
          {
            to:'docs/Usage/HowToInstall',
            activeBasePath:'docs/Usage/HowToInstall',
            label: 'Docs',
            position: 'left'
          },
          {to: '/blog', label: 'Blog', position: 'left'},
          {
            href: 'https://github.com/becat-oss',
            label: 'GitHub',
            position: 'right',
          },
          {
            href: 'https://becat.kyushu-u.ac.jp',
            label: 'About',
            position: 'right',
          },
        ],
      },
      footer: {
        style: 'dark',
        links: [
          {
            title: 'Docs',
            items: [
              {
                label: 'Component',
                to: '/docs/Component/Modelling',
              },
              {
                label: 'Usage',
                to: '/docs/Component/CreateTherbModel',
              },
            ],
          },
          {
            title: 'Community',
            items: [
              {
                label: 'Food4Rhino',
                href: 'https://www.food4rhino.com/app/hoaryfox',
              },
              {
                label: 'Twitter',
                href: 'https://twitter.com/katsuyaobara',
              },
            ],
          },
          {
            title: 'More',
            items: [
              {
                label: 'Blog',
                to: '/blog',
              },
              {
                label: 'Donation',
                href: 'https://github.com/facebook/docusaurus',
              },
            ],
          },
        ],
        copyright: `Copyright © ${new Date().getFullYear()} THERB2.0, Inc. Built with Docusaurus.`,
      },
      prism: {
        theme: lightCodeTheme,
        darkTheme: darkCodeTheme,
      },
    }),
};

module.exports = config;
