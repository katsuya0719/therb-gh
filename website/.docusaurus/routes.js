
import React from 'react';
import ComponentCreator from '@docusaurus/ComponentCreator';

export default [
  {
    path: '/therb-gh/__docusaurus/debug',
    component: ComponentCreator('/therb-gh/__docusaurus/debug','576'),
    exact: true
  },
  {
    path: '/therb-gh/__docusaurus/debug/config',
    component: ComponentCreator('/therb-gh/__docusaurus/debug/config','e39'),
    exact: true
  },
  {
    path: '/therb-gh/__docusaurus/debug/content',
    component: ComponentCreator('/therb-gh/__docusaurus/debug/content','c41'),
    exact: true
  },
  {
    path: '/therb-gh/__docusaurus/debug/globalData',
    component: ComponentCreator('/therb-gh/__docusaurus/debug/globalData','da6'),
    exact: true
  },
  {
    path: '/therb-gh/__docusaurus/debug/metadata',
    component: ComponentCreator('/therb-gh/__docusaurus/debug/metadata','456'),
    exact: true
  },
  {
    path: '/therb-gh/__docusaurus/debug/registry',
    component: ComponentCreator('/therb-gh/__docusaurus/debug/registry','18e'),
    exact: true
  },
  {
    path: '/therb-gh/__docusaurus/debug/routes',
    component: ComponentCreator('/therb-gh/__docusaurus/debug/routes','fa4'),
    exact: true
  },
  {
    path: '/therb-gh/blog',
    component: ComponentCreator('/therb-gh/blog','b62'),
    exact: true
  },
  {
    path: '/therb-gh/blog/archive',
    component: ComponentCreator('/therb-gh/blog/archive','c78'),
    exact: true
  },
  {
    path: '/therb-gh/blog/class',
    component: ComponentCreator('/therb-gh/blog/class','6ae'),
    exact: true
  },
  {
    path: '/therb-gh/blog/development',
    component: ComponentCreator('/therb-gh/blog/development','6a7'),
    exact: true
  },
  {
    path: '/therb-gh/blog/release',
    component: ComponentCreator('/therb-gh/blog/release','a00'),
    exact: true
  },
  {
    path: '/therb-gh/blog/tags',
    component: ComponentCreator('/therb-gh/blog/tags','c35'),
    exact: true
  },
  {
    path: '/therb-gh/blog/tags/announcement',
    component: ComponentCreator('/therb-gh/blog/tags/announcement','191'),
    exact: true
  },
  {
    path: '/therb-gh/blog/tags/development',
    component: ComponentCreator('/therb-gh/blog/tags/development','e82'),
    exact: true
  },
  {
    path: '/therb-gh/blog/tags/lecture',
    component: ComponentCreator('/therb-gh/blog/tags/lecture','163'),
    exact: true
  },
  {
    path: '/therb-gh/markdown-page',
    component: ComponentCreator('/therb-gh/markdown-page','325'),
    exact: true
  },
  {
    path: '/therb-gh/docs',
    component: ComponentCreator('/therb-gh/docs','9c7'),
    routes: [
      {
        path: '/therb-gh/docs/Component/Filter',
        component: ComponentCreator('/therb-gh/docs/Component/Filter','41d'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Component/IO',
        component: ComponentCreator('/therb-gh/docs/Component/IO','b14'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Component/Modelling',
        component: ComponentCreator('/therb-gh/docs/Component/Modelling','64c'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Component/Simulation',
        component: ComponentCreator('/therb-gh/docs/Component/Simulation','63c'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Component/Utility',
        component: ComponentCreator('/therb-gh/docs/Component/Utility','883'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Detail/Concept',
        component: ComponentCreator('/therb-gh/docs/Detail/Concept','8e8'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/intro',
        component: ComponentCreator('/therb-gh/docs/intro','016'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/NewHasp/HowToUse',
        component: ComponentCreator('/therb-gh/docs/NewHasp/HowToUse','cda'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Reference/DefaultParameter',
        component: ComponentCreator('/therb-gh/docs/Reference/DefaultParameter','7a4'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Usage/CreateTherbModel',
        component: ComponentCreator('/therb-gh/docs/Usage/CreateTherbModel','388'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Usage/EditTherbModel',
        component: ComponentCreator('/therb-gh/docs/Usage/EditTherbModel','208'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Usage/HowToInstall',
        component: ComponentCreator('/therb-gh/docs/Usage/HowToInstall','459'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Usage/RegisterConstruction',
        component: ComponentCreator('/therb-gh/docs/Usage/RegisterConstruction','9cd'),
        exact: true,
        'sidebar': "tutorialSidebar"
      },
      {
        path: '/therb-gh/docs/Usage/RunSimulation',
        component: ComponentCreator('/therb-gh/docs/Usage/RunSimulation','2b3'),
        exact: true,
        'sidebar': "tutorialSidebar"
      }
    ]
  },
  {
    path: '/therb-gh/',
    component: ComponentCreator('/therb-gh/','bb3'),
    exact: true
  },
  {
    path: '*',
    component: ComponentCreator('*')
  }
];
