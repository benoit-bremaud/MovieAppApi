#!/bin/bash
echo "🚀 Starting MovieApp Backend & Documentation..."

# Start API in background
cd MovieAppApi
dotnet run &
API_PID=$!

# Go back to root and start DocFX
cd ..
~/.dotnet/tools/docfx docfx.json --serve &
DOC_PID=$!

echo "✅ API running on http://localhost:5174"
echo "✅ Documentation running on http://localhost:8080"
echo "Press CTRL+C to stop everything."

trap "kill $API_PID $DOC_PID" EXIT
wait
