var status = {
    "host": "2013-20160416VV",
    "advisoryHostFQDNs": [],
    "version": "3.2.6",
    "process": "C:\\Program Files\\MongoDB\\Server\\3.2\\bin\\mongod.exe",
    "pid": { "$numberLong": "4308" },
    "uptime": 536,
    "uptimeMillis": { "$numberLong": "536597" },
    "uptimeEstimate": 536,
    "localTime": { "$date": "2016-07-21T07:24:15.651+0800" },
    "asserts": {
        "regular": 0,
        "warning": 0,
        "msg": 0,
        "user": 0,
        "rollovers": 0
    },
    "connections": {
        "current": 4,
        "available": 999996,
        "totalCreated": { "$numberLong": "15" }
    },
    "extra_info": {
        "note": "fields vary by platform",
        "page_faults": 49420,
        "usagePageFileMB": 73,
        "totalPageFileMB": 16145,
        "availPageFileMB": 12562,
        "ramMB": 8073
    },
    "globalLock": //全局写入锁占用了服务器多少时间（以微秒计）
        {
            "totalTime": { "$numberLong": "536596000" },
            "currentQueue": {
                "total": 0,
                "readers": 0,
                "writers": 0
            },
            "activeClients": {
                "total": 12,
                "readers": 0,
                "writers": 0
            }
        },
    "locks": {
        "Global": {
            "acquireCount": {
                "r": { "$numberLong": "2229" },
                "w": { "$numberLong": "10" },
                "R": { "$numberLong": "1" },
                "W": { "$numberLong": "4" }
            }
        },
        "Database": {
            "acquireCount": {
                "r": { "$numberLong": "1105" },
                "R": { "$numberLong": "2" },
                "W": { "$numberLong": "10" }
            }
        },
        "Collection": { "acquireCount": { "r": { "$numberLong": "1151" } } },
        "Metadata": { "acquireCount": { "w": { "$numberLong": "1" } } }
    },
    "network": {
        "bytesIn": { "$numberLong": "2006" },
        "bytesOut": { "$numberLong": "1954" },
        "numRequests": { "$numberLong": "35" }
    },
    "opcounters": //重要，包含了每种主要操作的次数
        {
            "insert": 0,
            "query": 1,
            "update": 0,
            "delete": 0,
            "getmore": 0,
            "command": 40
        },
    "opcountersRepl": {
        "insert": 0,
        "query": 0,
        "update": 0,
        "delete": 0,
        "getmore": 0,
        "command": 0
    },
    "storageEngine": {
        "name": "wiredTiger",
        "supportsCommittedReads": true,
        "persistent": true
    },
    "tcmalloc": {
        "generic": {
            "current_allocated_bytes": 53480560,
            "heap_size": 55508992
        },
        "tcmalloc": {
            "pageheap_free_bytes": 1032192,
            "pageheap_unmapped_bytes": 8192,
            "max_total_thread_cache_bytes": { "$numberLong": "1073741824" },
            "current_total_thread_cache_bytes": 405432,
            "central_cache_free_bytes": 582616,
            "transfer_cache_free_bytes": 0,
            "thread_cache_free_bytes": 405432,
            "aggressive_memory_decommit": 0
        },
        "formattedString": "------------------------------------------------\nMALLOC:       53480560 (   51.0 MiB) Bytes in use by application\nMALLOC: +      1032192 (    1.0 MiB) Bytes in page heap freelist\nMALLOC: +       582616 (    0.6 MiB) Bytes in central cache freelist\nMALLOC: +            0 (    0.0 MiB) Bytes in transfer cache freelist\nMALLOC: +       405432 (    0.4 MiB) Bytes in thread cache freelists\nMALLOC: +      4206752 (    4.0 MiB) Bytes in malloc metadata\nMALLOC:   ------------\nMALLOC: =     59707552 (   56.9 MiB) Actual memory used (physical + swap)\nMALLOC: +         8192 (    0.0 MiB) Bytes released to OS (aka unmapped)\nMALLOC:   ------------\nMALLOC: =     59715744 (   56.9 MiB) Virtual address space used\nMALLOC:\nMALLOC:            242              Spans in use\nMALLOC:              6              Thread heaps in use\nMALLOC:           8192              Tcmalloc page size\n------------------------------------------------\nCall ReleaseFreeMemory() to release freelist memory to the OS (via madvise()).\nBytes released to the OS take up virtual address space but no physical memory.\n"
    },
    "wiredTiger": {
        "uri": "statistics:",
        "LSM": {
            "application work units currently queued": 0,
            "merge work units currently queued": 0,
            "rows merged in an LSM tree": 0,
            "sleep for LSM checkpoint throttle": 0,
            "sleep for LSM merge throttle": 0,
            "switch work units currently queued": 0,
            "tree maintenance operations discarded": 0,
            "tree maintenance operations executed": 0,
            "tree maintenance operations scheduled": 0,
            "tree queue hit maximum": 0
        },
        "async": {
            "current work queue length": 0,
            "maximum work queue length": 0,
            "number of allocation state races": 0,
            "number of flush calls": 0,
            "number of operation slots viewed for allocation": 0,
            "number of times operation allocation failed": 0,
            "number of times worker found no work": 0,
            "total allocations": 0,
            "total compact calls": 0,
            "total insert calls": 0,
            "total remove calls": 0,
            "total search calls": 0,
            "total update calls": 0
        },
        "block-manager": {
            "blocks pre-loaded": 26,
            "blocks read": 67,
            "blocks written": 25,
            "bytes read": 315392,
            "bytes written": 163840,
            "mapped blocks read": 0,
            "mapped bytes read": 0
        },
        "cache": {
            "bytes currently in the cache": 110926,
            "bytes read into cache": 65837,
            "bytes written from cache": 99680,
            "checkpoint blocked page eviction": 0,
            "eviction currently operating in aggressive mode": 0,
            "eviction server candidate queue empty when topping up": 0,
            "eviction server candidate queue not empty when topping up": 0,
            "eviction server evicting pages": 0,
            "eviction server populating queue, but not evicting pages": 0,
            "eviction server unable to reach eviction goal": 0,
            "eviction worker thread evicting pages": 0,
            "failed eviction of pages that exceeded the in-memory maximum": 0,
            "hazard pointer blocked page eviction": 0,
            "in-memory page passed criteria to be split": 0,
            "in-memory page splits": 0,
            "internal pages evicted": 0,
            "internal pages split during eviction": 0,
            "leaf pages split during eviction": 0,
            "lookaside table insert calls": 0,
            "lookaside table remove calls": 0,
            "maximum bytes configured": 4294967296,
            "maximum page size at eviction": 0,
            "modified pages evicted": 0,
            "page split during eviction deepened the tree": 0,
            "page written requiring lookaside records": 0,
            "pages currently held in the cache": 25,
            "pages evicted because they exceeded the in-memory maximum": 0,
            "pages evicted because they had chains of deleted items": 0,
            "pages evicted by application threads": 0,
            "pages read into cache": 24,
            "pages read into cache requiring lookaside entries": 0,
            "pages selected for eviction unable to be evicted": 0,
            "pages walked for eviction": 0,
            "pages written from cache": 13,
            "pages written requiring in-memory restoration": 0,
            "percentage overhead": 8,
            "tracked bytes belonging to internal pages in the cache": 15995,
            "tracked bytes belonging to leaf pages in the cache": 94931,
            "tracked bytes belonging to overflow pages in the cache": 0,
            "tracked dirty bytes in the cache": 0,
            "tracked dirty pages in the cache": 0,
            "unmodified pages evicted": 0
        },
        "connection": {
            "auto adjusting condition resets": 10,
            "auto adjusting condition wait calls": 1650,
            "files currently open": 16,
            "memory allocations": 11694,
            "memory frees": 10818,
            "memory re-allocations": 1703,
            "pthread mutex condition wait calls": 7084,
            "pthread mutex shared lock read-lock calls": 1372,
            "pthread mutex shared lock write-lock calls": 726,
            "total read I/Os": 504,
            "total write I/Os": 40
        },
        "cursor": {
            "cursor create calls": 42,
            "cursor insert calls": 18,
            "cursor next calls": 103,
            "cursor prev calls": 10,
            "cursor remove calls": 1,
            "cursor reset calls": 421,
            "cursor restarted searches": 0,
            "cursor search calls": 411,
            "cursor search near calls": 1,
            "cursor update calls": 0,
            "truncate calls": 0
        },
        "data-handle": {
            "connection data handles currently active": 13,
            "connection sweep candidate became referenced": 0,
            "connection sweep dhandles closed": 0,
            "connection sweep dhandles removed from hash list": 88,
            "connection sweep time-of-death sets": 88,
            "connection sweeps": 53,
            "session dhandles swept": 0,
            "session sweep attempts": 22
        },
        "log": {
            "busy returns attempting to switch slots": 0,
            "consolidated slot closures": 8,
            "consolidated slot join races": 0,
            "consolidated slot join transitions": 8,
            "consolidated slot joins": 11,
            "consolidated slot unbuffered writes": 0,
            "log bytes of payload data": 3690,
            "log bytes written": 5120,
            "log files manually zero-filled": 0,
            "log flush operations": 5363,
            "log force write operations": 5926,
            "log force write operations skipped": 5923,
            "log records compressed": 5,
            "log records not compressed": 0,
            "log records too small to compress": 6,
            "log release advances write LSN": 5,
            "log scan operations": 3,
            "log scan records requiring two reads": 4,
            "log server thread advances write LSN": 3,
            "log server thread write LSN walk skipped": 1485,
            "log sync operations": 8,
            "log sync_dir operations": 1,
            "log write operations": 11,
            "logging bytes consolidated": 4736,
            "maximum log file size": 104857600,
            "number of pre-allocated log files to create": 2,
            "pre-allocated log files not ready and missed": 1,
            "pre-allocated log files prepared": 2,
            "pre-allocated log files used": 0,
            "records processed by log scan": 10,
            "total in-memory size of compressed records": 5459,
            "total log buffer size": 33554432,
            "total size of compressed records": 3546,
            "written slots coalesced": 0,
            "yields waiting for previous log file close": 0
        },
        "reconciliation": {
            "fast-path pages deleted": 0,
            "page reconciliation calls": 12,
            "page reconciliation calls for eviction": 0,
            "pages deleted": 0,
            "split bytes currently awaiting free": 0,
            "split objects currently awaiting free": 0
        },
        "session": {
            "open cursor count": 26,
            "open session count": 14
        },
        "thread-yield": {
            "page acquire busy blocked": 0,
            "page acquire eviction blocked": 0,
            "page acquire locked blocked": 0,
            "page acquire read blocked": 0,
            "page acquire time sleeping (usecs)": 0
        },
        "transaction": {
            "number of named snapshots created": 0,
            "number of named snapshots dropped": 0,
            "transaction begins": 23,
            "transaction checkpoint currently running": 0,
            "transaction checkpoint generation": 9,
            "transaction checkpoint max time (msecs)": 14,
            "transaction checkpoint min time (msecs)": 1,
            "transaction checkpoint most recent time (msecs)": 2,
            "transaction checkpoint total time (msecs)": 42,
            "transaction checkpoints": 9,
            "transaction failures due to cache overflow": 0,
            "transaction range of IDs currently pinned": 0,
            "transaction range of IDs currently pinned by a checkpoint": 0,
            "transaction range of IDs currently pinned by named snapshots": 0,
            "transaction sync calls": 0,
            "transactions committed": 3,
            "transactions rolled back": 20
        },
        "concurrentTransactions": {
            "write": {
                "out": 0,
                "available": 128,
                "totalTickets": 128
            },
            "read": {
                "out": 0,
                "available": 128,
                "totalTickets": 128
            }
        }
    },
    "writeBacksQueued": false,
    "mem": //服务器内存映射了多少数据，服务器进程的虚拟内存和常驻内存的占用情况（单位是MB）
        {
            "bits": 64,
            "resident": 45,
            "virtual": 172,
            "supported": true,
            "mapped": 0,
            "mappedWithJournal": 0
        },
    "metrics": {
        "commands": {
            "<UNKNOWN>": { "$numberLong": "0" },
            "_getUserCacheGeneration": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "_isSelf": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "_mergeAuthzCollections": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "_migrateClone": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "_recvChunkAbort": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "_recvChunkCommit": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "_recvChunkStart": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "_recvChunkStatus": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "_transferMods": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "aggregate": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "appendOplogNote": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "applyOps": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "authSchemaUpgrade": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "authenticate": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "availableQueryOptions": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "buildInfo": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "checkShardingIndex": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "cleanupOrphaned": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "clone": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "cloneCollection": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "cloneCollectionAsCapped": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "collMod": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "collStats": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "compact": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "connPoolStats": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "connPoolSync": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "connectionStatus": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "convertToCapped": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "copydb": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "copydbgetnonce": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "copydbsaslstart": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "count": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "create": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "createIndexes": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "createRole": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "createUser": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "currentOp": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "currentOpCtx": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dataSize": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dbHash": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dbStats": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "delete": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "diagLogging": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "distinct": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "driverOIDTest": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "drop": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dropAllRolesFromDatabase": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dropAllUsersFromDatabase": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dropDatabase": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dropIndexes": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dropRole": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "dropUser": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "eval": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "explain": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "features": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "filemd5": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "find": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "findAndModify": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "forceerror": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "fsync": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "fsyncUnlock": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "geoNear": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "geoSearch": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getCmdLineOpts": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getLastError": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getLog": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getMore": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getParameter": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getPrevError": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getShardMap": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getShardVersion": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "getnonce": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "grantPrivilegesToRole": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "grantRolesToRole": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "grantRolesToUser": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "group": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "handshake": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "hostInfo": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "insert": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "invalidateUserCache": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "isMaster": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "4" }
            },
            "killCursors": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "killOp": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "listCollections": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "listCommands": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "listDatabases": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "1" }
            },
            "listIndexes": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "logRotate": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "logout": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "mapReduce": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "mapreduce": {
                "shardedfinish": {
                    "failed": { "$numberLong": "0" },
                    "total": { "$numberLong": "0" }
                }
            },
            "mergeChunks": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "moveChunk": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "parallelCollectionScan": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "ping": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "31" }
            },
            "planCacheClear": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "planCacheClearFilters": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "planCacheListFilters": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "planCacheListPlans": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "planCacheListQueryShapes": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "planCacheSetFilter": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "profile": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "reIndex": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "renameCollection": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "repairCursor": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "repairDatabase": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetDeclareElectionWinner": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetElect": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetFreeze": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetFresh": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetGetConfig": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetGetRBID": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetGetStatus": {
                "failed": { "$numberLong": "1" },
                "total": { "$numberLong": "1" }
            },
            "replSetHeartbeat": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetInitiate": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetMaintenance": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetReconfig": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetRequestVotes": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetStepDown": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetSyncFrom": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "replSetUpdatePosition": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "resetError": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "resync": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "revokePrivilegesFromRole": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "revokeRolesFromRole": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "revokeRolesFromUser": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "rolesInfo": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "saslContinue": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "saslStart": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "serverStatus": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "2" }
            },
            "setParameter": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "setShardVersion": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "shardConnPoolStats": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "shardingState": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "shutdown": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "splitChunk": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "splitVector": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "top": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "1" }
            },
            "touch": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "unsetSharding": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "update": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "updateRole": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "updateUser": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "usersInfo": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "validate": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "whatsmyuri": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            },
            "writebacklisten": {
                "failed": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            }
        },
        "cursor": {
            "timedOut": { "$numberLong": "0" },
            "open": {
                "noTimeout": { "$numberLong": "0" },
                "pinned": { "$numberLong": "0" },
                "total": { "$numberLong": "0" }
            }
        },
        "document": {
            "deleted": { "$numberLong": "0" },
            "inserted": { "$numberLong": "0" },
            "returned": { "$numberLong": "0" },
            "updated": { "$numberLong": "0" }
        },
        "getLastError": {
            "wtime": {
                "num": 0,
                "totalMillis": 0
            },
            "wtimeouts": { "$numberLong": "0" }
        },
        "operation": {
            "fastmod": { "$numberLong": "0" },
            "idhack": { "$numberLong": "0" },
            "scanAndOrder": { "$numberLong": "0" },
            "writeConflicts": { "$numberLong": "0" }
        },
        "queryExecutor": {
            "scanned": { "$numberLong": "0" },
            "scannedObjects": { "$numberLong": "0" }
        },
        "record": { "moves": { "$numberLong": "0" } },
        "repl": {
            "executor": {
                "counters": {
                    "eventCreated": 0,
                    "eventWait": 0,
                    "cancels": 0,
                    "waits": 0,
                    "scheduledNetCmd": 0,
                    "scheduledDBWork": 0,
                    "scheduledXclWork": 0,
                    "scheduledWorkAt": 0,
                    "scheduledWork": 0,
                    "schedulingFailures": 0
                },
                "queues": {
                    "networkInProgress": 0,
                    "dbWorkInProgress": 0,
                    "exclusiveInProgress": 0,
                    "sleepers": 0,
                    "ready": 0,
                    "free": 0
                },
                "unsignaledEvents": 0,
                "eventWaiters": 0,
                "shuttingDown": false,
                "networkInterface": "NetworkInterfaceASIO inShutdown: 0"
            },
            "apply": {
                "batches": {
                    "num": 0,
                    "totalMillis": 0
                },
                "ops": { "$numberLong": "0" }
            },
            "buffer": {
                "count": { "$numberLong": "0" },
                "maxSizeBytes": 268435456,
                "sizeBytes": { "$numberLong": "0" }
            },
            "network": {
                "bytes": { "$numberLong": "0" },
                "getmores": {
                    "num": 0,
                    "totalMillis": 0
                },
                "ops": { "$numberLong": "0" },
                "readersCreated": { "$numberLong": "0" }
            },
            "preload": {
                "docs": {
                    "num": 0,
                    "totalMillis": 0
                },
                "indexes": {
                    "num": 0,
                    "totalMillis": 0
                }
            }
        },
        "storage": {
            "freelist": {
                "search": {
                    "bucketExhausted": { "$numberLong": "0" },
                    "requests": { "$numberLong": "0" },
                    "scanned": { "$numberLong": "0" }
                }
            }
        },
        "ttl": {
            "deletedDocuments": { "$numberLong": "0" },
            "passes": { "$numberLong": "8" }
        }
    },
    "ok": 1
}