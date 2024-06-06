# Util functionality

## Overview

This file will containt the docs for speciaal functions that do not directly
fit in to other services and repositories.

### Storing images

The UploadImageToCloudflare function is responsible for storing images for the
application. It uploads images to Cloudflare's R2 bucket and retrieves the URL
where the image is hosted. This URL is then associated with the corresponding
card in the application.

**Process**

- Image Upload: The function receives an image to be uploaded.
- Cloudflare R2 Bucket: The image is uploaded to Cloudflare's R2 bucket.
- JavaScript Worker: A predefined JavaScript worker processes the image upload.
- Image URL Retrieval: The JavaScript worker returns the URL of the hosted image.
- URL Storage: The returned image URL is stored with the card that was created.

**Benefits**

- Cost Efficiency: Cloudflare is chosen over Azure due to its cost-effectiveness.
