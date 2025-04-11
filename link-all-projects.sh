#!/bin/bash

MAIN_SOLUTION="AwsFundamentals.sln"

# Exit if the main solution doesn't exist
if [ ! -f "$MAIN_SOLUTION" ]; then
  echo "❌ Solution file $MAIN_SOLUTION not found!"
  exit 1
fi

# Find all .csproj files in all subfolders (excluding the main solution's root)
find . -type f -name "*.csproj" ! -path "./$MAIN_SOLUTION" | while read proj; do
  if grep -Fq "$(basename "$proj")" "$MAIN_SOLUTION"; then
    echo "✅ Already added: $proj"
  else
    echo "➕ Adding: $proj"
    dotnet sln "$MAIN_SOLUTION" add "$proj"
  fi
done
