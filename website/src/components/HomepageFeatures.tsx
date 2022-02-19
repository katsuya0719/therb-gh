import useBaseUrl from '@docusaurus/useBaseUrl';
import React from 'react';
import clsx from 'clsx';
import styles from './HomepageFeatures.module.css';

type FeatureItem = {
  title: string;
  image: string;
  description: JSX.Element;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Easy to Use',
    image: '/img/undraw_docusaurus_mountain.svg',
    description: (
      <>
        THERB 2.0は温熱環境シミュレーションTHERBを簡単に使えるインターフェースです
      </>
    ),
  },
  {
    title: 'Focus on efficiency improvement',
    image: '/img/undraw_docusaurus_tree.svg',
    description: (
      <>
        煩雑なモデリング工程、時間のかかる後処理工程を自動化することで、ユーザーの生産性の向上に寄与します
      </>
    ),
  },
  {
    title: 'Enhance interoperability',
    image: '/img/undraw_docusaurus_react.svg',
    description: (
      <>
        THERB以外にも、New-Hasp、BAUES Analysisなど異なるシミュレーションツールに書き出しが可能です
      </>
    ),
  },
];

function Feature({title, image, description}: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center">
        <img
          className={styles.featureSvg}
          alt={title}
          src={useBaseUrl(image)}
        />
      </div>
      <div className="text--center padding-horiz--md">
        <h3>{title}</h3>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): JSX.Element {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
