#!/bin/bash

export PROJECT_ID=$(gcloud config get-value project)

echo Running container builder
gcloud builds submit --config cloudbuild.yaml .