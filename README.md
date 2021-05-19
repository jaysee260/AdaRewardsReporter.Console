# ADA Rewards Reporter

## About

A console application for reporting on total ADA staking rewards earned to date, broken down by epoch. Output is printed on the console in the form of a table.

The [Blockfrost](https://blockfrost.io/) API is used to query the [Cardano](https://cardano.org/) blockchain.

## Configuration

An `appsettings.json` or `appsettings.Local.json` file, with the following values, is necessary for the app to run. `appsettings.Local.json` is used to avoid commiting the Blockfrost API Key to source control. Alternatively, these configuration values can also be set as environment variables.

```json
{
    "AuthenticationHeaderKey": "project_id",
    "ApiKey": "<redacted>",
    "MainnetBaseUrl": "https://cardano-mainnet.blockfrost.io/api/v0"
}
```

To get an API key, you can create a free developer account at [blockfrost.io](https://blockfrost.io/).

## Parameters

### stakeAddress (_required_)
The stake address of the wallet from which ADA is being staked. AFAIK, [Daedalus](https://daedaluswallet.io/[) and [Yoroi](https://yoroi-wallet.com/#/) give you access to your stake address, but [Exodus](https://www.exodus.com/ada-cardano-wallet) doesn't.

### regularAddress (_optional_ | _required if provided instead of stakeAddress_)
A regular Cardano address used to send and receive ADA. If provided instead of the stake address, it will be used to resolve the stake address associated with the account. If provided in tandem with the stake address, it will be ignored. Using `regularAddress` instead of `stakeAddress` can be useful when staking in a wallet like Exodus where you don't have direct access to your stake address.

### orderBy (_optional_)
The order, by epoch, in which to show rewards history in the report. The default value is `desc`. Possible values are `desc` or `asc`.

### exportToCsv (_optional_)
Setting this flag will output a CSV file to the project's root directory with the contents of the report.

## How to run

```
dotnet run -- --stakeAddress=<your_stake_address>
```

```
dotnet run -- --stakeAddress=<your_stake_address> --exportToCsv
```
