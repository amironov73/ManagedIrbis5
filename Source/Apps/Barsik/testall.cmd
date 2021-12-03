@echo off

for %%q in (*.barsik) do (
echo ======================
echo %%q
barsik %%q
echo ======================
)
