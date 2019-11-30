#ifndef CODESIDE_CONSTANTS_H
#define CODESIDE_CONSTANTS_H

constexpr const int MAX_TICK_COUNT = 10000;
constexpr const double TICKS_PER_SECOND = 60;
constexpr const int UPDATES_PER_TICK = 100;
constexpr const double LOOT_BOX_SIZE = 0.5;
constexpr const double UNIT_SIZE_X = 0.9;
constexpr const double UNIT_SIZE_Y = 1.8;
constexpr const double UNIT_MAX_HORIZONTAL_SPEED = 10;
constexpr const double UNIT_FALL_SPEED = 10;
constexpr const double UNIT_JUMP_TIME = 0.55;
constexpr const double UNIT_JUMP_SPEED = 10;
constexpr const double JUMP_PAD_JUMP_TIME = 0.525;
constexpr const double JUMP_PAD_JUMP_SPEED = 20;
constexpr const int UNIT_MAX_HEALTH = 100;
constexpr const int HEALTH_PACK_HEALTH = 50;
constexpr const double MINE_SIZE = 0.5;
constexpr const double MINE_PREPARE_TIME = 1;
constexpr const double MINE_TRIGGER_TIME = 0.5;
constexpr const double MINE_TRIGGER_RADIUS = 1;
constexpr const int KILL_SCORE = 100;
constexpr const double MINE_EXPLOSION_RADIUS = 3;
constexpr const int MINE_EXPLOSION_DAMAGE = 50;

constexpr const int PISTOL_MAGAZINE_SIZE = 8;
constexpr const double PISTOL_FIRE_RATE = 0.4;
constexpr const double PISTOL_RELOAD_TIME = 1;
constexpr const double PISTOL_MIN_SPREAD = 0.05;
constexpr const double PISTOL_MAX_SPREAD = 0.7;
constexpr const double PISTOL_RECOIL = 0.5;
constexpr const double PISTOL_AIM_SPEED = 1;
constexpr const double PISTOL_BULLET_SPEED = 50;
constexpr const double PISTOL_BULLET_SIZE = 0.2;
constexpr const int PISTOL_BULLET_DAMAGE = 20;
constexpr const int ASSAULT_RIFLE_MAGAZINE_SIZE = 20;
constexpr const double ASSAULT_RIFLE_FIRE_RATE = 0.1;
constexpr const double ASSAULT_RIFLE_RELOAD_TIME = 1;
constexpr const double ASSAULT_RIFLE_MIN_SPREAD = 0.1;
constexpr const double ASSAULT_RIFLE_MAX_SPREAD = 0.7;
constexpr const double ASSAULT_RIFLE_RECOIL = 0.2;
constexpr const double ASSAULT_RIFLE_AIM_SPEED = 1.9;
constexpr const double ASSAULT_RIFLE_BULLET_SPEED = 50;
constexpr const double ASSAULT_RIFLE_BULLET_SIZE = 0.2;
constexpr const int ASSAULT_RIFLE_BULLET_DAMAGE = 5;
constexpr const int ROCKET_LAUNCHER_MAGAZINE_SIZE = 1;
constexpr const double ROCKET_LAUNCHER_FIRE_RATE = 1;
constexpr const double ROCKET_LAUNCHER_RELOAD_TIME = 1;
constexpr const double ROCKET_LAUNCHER_MIN_SPREAD = 0.1;
constexpr const double ROCKET_LAUNCHER_MAX_SPREAD = 0.7;
constexpr const double ROCKET_LAUNCHER_RECOIL = 1;
constexpr const double ROCKET_LAUNCHER_AIM_SPEED = 0.5;
constexpr const double ROCKET_LAUNCHER_BULLET_SPEED = 20;
constexpr const double ROCKET_LAUNCHER_BULLET_SIZE = 0.4;
constexpr const int ROCKET_LAUNCHER_BULLET_DAMAGE = 30;

#endif //CODESIDE_CONSTANTS_H