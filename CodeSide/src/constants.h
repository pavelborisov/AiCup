#ifndef CODESIDE_CONSTANTS_H
#define CODESIDE_CONSTANTS_H

#define M_MICROTICK_IS_TICK 0
#define M_NO_SPREAD 0

#if M_MICROTICK_IS_TICK
constexpr const int MAX_TICK_COUNT = 360000;
constexpr const double TICKS_PER_SECOND = 6000;
constexpr const int UPDATES_PER_TICK = 1;
#else
constexpr const int MAX_TICK_COUNT = 3600;
constexpr const double TICKS_PER_SECOND = 60;
constexpr const int UPDATES_PER_TICK = 100;
#endif
constexpr const double LOOT_BOX_SIZE = 0.5;
constexpr const double UNIT_WIDTH = 0.9;
constexpr const double UNIT_HEIGHT = 1.8;
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
constexpr const int KILL_SCORE = 1000;
constexpr const double MINE_EXPLOSION_RADIUS = 3;
constexpr const int MINE_EXPLOSION_DAMAGE = 50;

constexpr const int PISTOL_MAGAZINE_SIZE = 8;
constexpr const double PISTOL_FIRE_RATE = 0.4;
constexpr const double PISTOL_RELOAD_TIME = 1;
constexpr const double PISTOL_RECOIL = 0.5;
constexpr const double PISTOL_AIM_SPEED = 1;
constexpr const double PISTOL_BULLET_SPEED = 50;
constexpr const double PISTOL_BULLET_SIZE = 0.2;
constexpr const int PISTOL_BULLET_DAMAGE = 20;
constexpr const int ASSAULT_RIFLE_MAGAZINE_SIZE = 20;
constexpr const double ASSAULT_RIFLE_FIRE_RATE = 0.1;
constexpr const double ASSAULT_RIFLE_RELOAD_TIME = 1;
constexpr const double ASSAULT_RIFLE_RECOIL = 0.2;
constexpr const double ASSAULT_RIFLE_AIM_SPEED = 1.9;
constexpr const double ASSAULT_RIFLE_BULLET_SPEED = 50;
constexpr const double ASSAULT_RIFLE_BULLET_SIZE = 0.2;
constexpr const int ASSAULT_RIFLE_BULLET_DAMAGE = 5;
constexpr const int ROCKET_LAUNCHER_MAGAZINE_SIZE = 1;
constexpr const double ROCKET_LAUNCHER_FIRE_RATE = 1;
constexpr const double ROCKET_LAUNCHER_RELOAD_TIME = 1;
constexpr const double ROCKET_LAUNCHER_RECOIL = 1;
constexpr const double ROCKET_LAUNCHER_AIM_SPEED = 1;
constexpr const double ROCKET_LAUNCHER_BULLET_SPEED = 20;
constexpr const double ROCKET_LAUNCHER_BULLET_SIZE = 0.4;
constexpr const int ROCKET_LAUNCHER_BULLET_DAMAGE = 30;
constexpr const int ROCKET_LAUNCHER_EXPLOSION_DAMAGE = 50;
constexpr const double ROCKET_LAUNCHER_EXPLOSION_RADIUS = 3;


#if M_NO_SPREAD
constexpr const double PISTOL_MIN_SPREAD = 0;
constexpr const double PISTOL_MAX_SPREAD = 0;
constexpr const double ASSAULT_RIFLE_MIN_SPREAD = 0;
constexpr const double ASSAULT_RIFLE_MAX_SPREAD = 0;
constexpr const double ROCKET_LAUNCHER_MIN_SPREAD = 0;
constexpr const double ROCKET_LAUNCHER_MAX_SPREAD = 0;
constexpr const double WEAPON_MAX_SPREAD = 0;
#else
constexpr const double PISTOL_MIN_SPREAD = 0.05;
constexpr const double PISTOL_MAX_SPREAD = 0.5;
constexpr const double ASSAULT_RIFLE_MIN_SPREAD = 0.1;
constexpr const double ASSAULT_RIFLE_MAX_SPREAD = 0.5;
constexpr const double ROCKET_LAUNCHER_MIN_SPREAD = 0.1;
constexpr const double ROCKET_LAUNCHER_MAX_SPREAD = 0.5;
constexpr const double WEAPON_MAX_SPREAD = 0.5;
#endif

constexpr const int WEAPON_DAMAGE[6] = {
        PISTOL_BULLET_DAMAGE,
        ASSAULT_RIFLE_BULLET_DAMAGE,
        ROCKET_LAUNCHER_BULLET_DAMAGE,
        0, 0, 0
};
constexpr const double WEAPON_AIM_SPEED[6] = {
        PISTOL_AIM_SPEED,
        ASSAULT_RIFLE_AIM_SPEED,
        ROCKET_LAUNCHER_AIM_SPEED,
        0, 0, 0
};
constexpr const double WEAPON_MIN_SPREAD[6] = {
        PISTOL_MIN_SPREAD,
        ASSAULT_RIFLE_MIN_SPREAD,
        ROCKET_LAUNCHER_MIN_SPREAD,
        0, 0, 0
};

constexpr const double UNIT_HALF_WIDTH = UNIT_WIDTH / 2;
constexpr const double UNIT_HALF_HEIGHT = UNIT_HEIGHT / 2;


constexpr const double WEAPON_RELOAD_TIME = 1;
static_assert(ASSAULT_RIFLE_RELOAD_TIME == WEAPON_RELOAD_TIME);
static_assert(PISTOL_RELOAD_TIME == WEAPON_RELOAD_TIME);
static_assert(ROCKET_LAUNCHER_RELOAD_TIME == WEAPON_RELOAD_TIME);

static_assert(PISTOL_MAX_SPREAD == WEAPON_MAX_SPREAD);
static_assert(ASSAULT_RIFLE_MAX_SPREAD == WEAPON_MAX_SPREAD);
static_assert(ROCKET_LAUNCHER_MAX_SPREAD == WEAPON_MAX_SPREAD);

#endif //CODESIDE_CONSTANTS_H