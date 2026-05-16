# Object Pooling System

The toolkit includes a reusable object pooling system designed to reduce runtime allocations and avoid repetitive Instantiate/Destroy operations.

Optimized for:
- floating rewards
- gameplay feedback systems
- reusable UI effects
- mobile gameplay workflows

---

# Pool Lifecycle

Spawn Request
      ↓
Pool Lookup
      ↓
Reuse Existing Object
      ↓
Activate Object
      ↓
Deactivate Object
      ↓
Return To Pool

---

# Optimization Goals

- reduce garbage collection spikes
- improve runtime performance
- minimize frame drops
- support reusable gameplay objects

---

# Demo

The demo includes:
- hierarchy reuse visualization
- pooled object recycling
- runtime activation/deactivation
- minimized object destruction