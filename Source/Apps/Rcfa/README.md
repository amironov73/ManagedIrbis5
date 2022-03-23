### Rcfa

Восстановление каталога из архивной копии.

База данных в ИРБИС64, как известно, состоит из двух частей: 1) собственно данных (мастер-файл плюс индексы), 2) "обвязка" (форматы, меню и пр.). Собственно данные восстанавливаются из RAR-архива, который устроен следующим образом:

```
├─┬─ATHRA
│ │
│ ├─athra.ifp
│ ├─athra.l01
│ ├─athra.mst
│ ├─athra.n01
│ └─athra.xrf
│
├─┬─ATHRB
│ │
│ ├─athrb.ifp
│ ├─athrb.l01
│ ├─athrb.mst
│ ├─athrb.n01
│ └─athrb.xrf
│
├─┬─ATHRC
│ │
│ ├─athrc.ifp
│ ├─athrc.l01
│ ├─athrc.mst
│ ├─athrc.n01
│ └─athrc.xrf
│
└─┬─IBIS
  │
  ├─ibis.ifp
  ├─ibis.l01
  ├─ibis.mst
  ├─ibis.n01
  └─ibis.xrf
```

Короче говоря, все данные помещены в папки, имена которых совпадают с именем базы.

"Обвязка" восстанавливается из актуальной "эталонной базы данных", как правило, это "IBIS" (но в вашем случае может быть любая другая).

Параметры командной строки:

```shell
Rcfa <archive> <irbis_server.ini> <original> <target> [ethalon]
```

здесь

* **archive** - имя rar-файла,
* **irbis_server.ini** - полный путь к файлу `irbis_server.ini` или полный путь к папке `Datai`,
* **original** - оригинальное название каталога (например, `IBIS`),
* **target** - новое название каталога (например, `IBIS_PREV`),
* **ethalon** - эталонная база данных, если не указана, то "IBIS".

Пример запуска:

```sh
Rcfa "E:\Backup\IOGUNB\data-2022-03-23-00-15.rar" "E:\IRBIS64_2015\Datai" IBIS IBIS_PREV
```

Пример вывода:

```
Clearing directory
Copying strapping stuff: !10.PFT !101.PFT !102.PFT !11.PFT !110.PFT !11S.PFT !19.PFT !210D.PFT !330n.pft
  !454n.pft !461.PFT !470n.pft !481n.pft !488n.pft !600.pft !601.pft !606.pft !607.pft !699.pft !700D.PFT
  !700dmars.pft !701.PFT !701dmars.pft !702.PFT !702dmars.pft !710D.PFT !790dmars.pft !900.PFT !901.PFT
  !903.PFT !907.PFT !910.PFT !910W.PFT !920.PFT !922n.pft !925n.pft !934.PFT !936.PFT !940W.PFT !963.PFT
  !964.PFT !amovi_mars.pft !amovi_mars_600.pft !boko.wss !bokodisc.wss !bokodiscO.wss !bokodiscV.wss
  VDU_SF.MNU VMARCI.FST VMARCI1.FST VMARCI2.FST VMARCI3.FST VMARCI4.FST VS600.fst WS.OPT WS31.OPT WS32.OPT
  ws42.opt ws42sist.opt ws52ko.opt zb.gbl zg.gbl zk_brief.ws znm.mnu ZO.GBL ZU.GBL
Copying data stuff:
    ibis.IFP done
    ibis.L01 done
    ibis.MST done
    ibis.N01 done
    ibis.XRF done
PAR file: IBIS_PREV.par
MNU file: dbnam1.mnu
MNU file: dbnam2.mnu
ALL DONE

```
