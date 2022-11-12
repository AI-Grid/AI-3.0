use robust;
CREATE TABLE IF NOT EXISTS tosauth (  
  avataruuid VARCHAR(36) NOT NULL, 
  agreed INT NOT NULL DEFAULT 0,
  avatarname VARCHAR(64),
  createtime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  confirmtime DATETIME NULL DEFAULT  CURRENT_TIMESTAMP,
  token VARCHAR(36) NOT NULL,
  UNIQUE INDEX uuid_UNIQUE (avataruuid ASC),
  UNIQUE INDEX avatarname_UNIQUE (avatarname ASC)
);

