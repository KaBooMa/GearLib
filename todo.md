# Features
- [ ] Create cutom motor parts
- [ ] Create custom gear parts
- [ ] Import custom materials into the game
- [ ] (STUCK) Create custom categories in the parts tab
- [x] Provide option to make configurable parts
- [ ] Provide access to the games default data type links via enum
- [ ] Add custom GUI options (Shift+E menu) for changing part values as a player
- [x] Provide modder direct access to the part properties to modify things such as density, paintable, swappable materials, etc.
- [ ] Proper colliders for custom models (Currently only generates Boxcolliders)
- [ ] Create a GearLib.Unity namespace for users to import into Unity, allowing creation of attachments and various other components directly to their prefab. This will allow easier placement of attachments versus writing it in code
- [ ] Resizable parts
- [ ] Add GUI buttons to various locations such as construction menu


# Bugs
- [x] Instancing issues w/ layers : Part.cs
- [ ] Random object reference null on startup, maybe on part adding? (This appears to be from creating data links. Not sure why this is or the fix yet.)
- [ ] Have had two individuals report crashing at startup. Might be related to the above or another GC issue
- [ ] Quicksave and possibly autosave do not currently save mod data
- [ ] Joystick field saves all values except the assigned key

# Documentation Improvements
- [x] Explain how to get your references from interop better
- [ ] Explain what the references do and why you need them