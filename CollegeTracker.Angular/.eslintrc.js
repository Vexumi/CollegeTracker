module.exports = {
  root: true,
  parser: '@typescript-eslint/parser',
  plugins: ['@typescript-eslint', '@angular-eslint', 'prettier', 'import'],
  extends: ['eslint:recommended', 'airbnb-typescript/base', 'prettier', 'plugin:@typescript-eslint/recommended', 'plugin:storybook/recommended'],
  env: {
    browser: true,
    node: true
  },
  parserOptions: {
    project: ['./tsconfig.json', './tsconfig.spec.json']
  },
  ignorePatterns: [
    '*/stories/**',
    '**/*.spec.ts',
  ],
  rules: {
    'arrow-body-style': 'off',
    'comma-dangle': ['error', 'never'],
    'indent': 'off',
    'linebreak-style': 'off',
    'max-len': ['warn', 120],
    'no-plusplus': 'off',
    'object-curly-newline': 'off',
    'quotes': ['error', 'single'],
    'no-cond-assign': ['error', 'always'],
    'no-console': 'off',
    'no-else-return': 'off',
    'eol-last': ['warn', 'always'],
    '@typescript-eslint/comma-dangle': ['error', 'never'],
    '@typescript-eslint/indent': ['off'],
    '@typescript-eslint/no-use-before-define': 'off',
    '@typescript-eslint/no-inferrable-types': 'off',
    '@typescript-eslint/lines-between-class-members': 'off',
    '@typescript-eslint/dot-notation': 'warn',
    '@typescript-eslint/explicit-member-accessibility': ['error', {
        accessibility: 'explicit',
        overrides: {
          constructors: 'no-public'
        }
    }],
    'prettier/prettier': 'error',
    'import/no-unresolved': ['error', {
      commonjs: true,
      caseSensitive: true
    }],
    'import/no-extraneous-dependencies': ['error'],
    'class-methods-use-this': 'off',
    'import/prefer-default-export': 'off',
    'import/no-unresolved': 'off',
    'dot-notation': 'off',
    '@typescript-eslint/dot-notation': 'off',
    'import/no-extraneous-dependencies': [
      'error',
      {
        'devDependencies': [
          '**/*.stories.*',
          '**/.storybook/**/*.*'
        ],
        'peerDependencies': true
      }
    ],
    '@angular-eslint/no-input-prefix': 'error',
    '@angular-eslint/contextual-decorator': 'error',
    '@angular-eslint/contextual-lifecycle': 'error',
    '@angular-eslint/directive-selector': 'error',
    '@angular-eslint/no-attribute-decorator': 'error',
    '@angular-eslint/no-conflicting-lifecycle': 'error',
    '@angular-eslint/no-empty-lifecycle-method': 'error',
    '@angular-eslint/no-input-prefix': 'error',
    '@angular-eslint/no-lifecycle-call': 'error',
    '@angular-eslint/no-queries-metadata-property': 'error',
    '@angular-eslint/prefer-on-push-component-change-detection': 'error',
    '@angular-eslint/no-output-native': 'error',
    '@angular-eslint/no-output-on-prefix': 'error',
    '@angular-eslint/prefer-output-readonly': 'error',
    '@angular-eslint/use-lifecycle-interface': 'error',
    '@angular-eslint/use-pipe-transform-interface': 'error',
  }
};