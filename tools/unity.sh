#!/usr/bin/env bash
# Batchmode helpers. The Unity editor must NOT have this project open while these run.
#   tools/unity.sh setup   -> runs ProjectSetup.RunFullSetup (settings, data, scenes)
#   tools/unity.sh rebuild -> overwrites the sample content with the values in ContentBuilder (after content changes)
#   tools/unity.sh economy -> writes Docs/ECONOMY.md from the content (after price or content changes)
#   tools/unity.sh test    -> runs EditMode tests, prints summary
#   tools/unity.sh playtest -> runs PlayMode tests (end-to-end loop, screenshots in Logs/batch/*.png)
#   tools/unity.sh android  -> development APK in Builds/Android (per-milestone build check)
#   tools/unity.sh compile -> opens the project once to compile, prints C# errors
set -u
UNITY="${UNITY:-C:/Program Files/Unity/Hub/Editor/6000.6.3f1/Editor/Unity.exe}"
PROJECT="$(cd "$(dirname "$0")/.." && pwd)"
OUT="$PROJECT/Logs/batch"
mkdir -p "$OUT"

case "${1:-}" in
  setup)
    "$UNITY" -batchmode -quit -projectPath "$PROJECT" \
      -executeMethod SortingGame.EditorTools.ProjectSetup.RunFullSetup -logFile "$OUT/setup.log"
    code=$?
    grep -E "error CS|Exception|\[ProjectSetup\]|\[ContentBuilder\]" "$OUT/setup.log"
    exit $code ;;
  rebuild)
    "$UNITY" -batchmode -quit -projectPath "$PROJECT" \
      -executeMethod SortingGame.EditorTools.ContentBuilder.Rebuild -logFile "$OUT/rebuild.log"
    code=$?
    grep -E "error CS|Exception|\[ContentBuilder\]" "$OUT/rebuild.log"
    exit $code ;;
  economy)
    "$UNITY" -batchmode -quit -projectPath "$PROJECT" \
      -executeMethod SortingGame.EditorTools.EconomyReport.Write -logFile "$OUT/economy.log"
    code=$?
    grep -E "error CS|Exception|\[EconomyReport\]" "$OUT/economy.log"
    exit $code ;;
  test|playtest)
    PLATFORM=EditMode; [ "$1" = playtest ] && PLATFORM=PlayMode
    "$UNITY" -batchmode -projectPath "$PROJECT" -runTests -testPlatform "$PLATFORM" \
      -testResults "$OUT/results.xml" -logFile "$OUT/tests.log"
    code=$?
    grep -E "error CS" "$OUT/tests.log"
    grep -oE '<test-run [^>]*>' "$OUT/results.xml" | grep -oE '(result|total|passed|failed)="[^"]*"' | tr '\n' ' '; echo
    grep -E '<test-case [^>]*result="Failed"' "$OUT/results.xml" | grep -oE 'fullname="[^"]*"'
    exit $code ;;
  android)
    "$UNITY" -batchmode -quit -projectPath "$PROJECT" \
      -executeMethod SortingGame.EditorTools.BuildTools.BuildAndroid -logFile "$OUT/android.log"
    code=$?
    grep -E "error CS|\[BuildTools\]|Error building|BuildFailedException|FAILURE:|What went wrong" -A3 "$OUT/android.log" | head -40
    exit $code ;;
  compile)
    "$UNITY" -batchmode -quit -projectPath "$PROJECT" -logFile "$OUT/compile.log"
    code=$?
    grep -E "error CS|warning CS" "$OUT/compile.log" | awk '!seen[$0]++'
    exit $code ;;
  *)
    echo "usage: tools/unity.sh setup|rebuild|economy|test|playtest|android|compile"; exit 2 ;;
esac
