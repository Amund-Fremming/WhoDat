update app.json

```json
"ios": {
      "supportsTablet": false,
      "bundleIdentifier": "com.amundfremming.whodat",
      "buildNumber": "1.0.0"
    },
```

Søk etter Certificates, Identifiers & Profiles og opprett en App ID
legg inn identifier som den over.

Under apps, opprett en ny app og fyll in SKU(kan være hva som helst)

i terminalen

- eas login
- eas build --platform ios

questions to answer

- use old distribution certificate? yes
- Generate a new apple provisioning profile? yes

i terminalen

- eas sumbit --platform ios
